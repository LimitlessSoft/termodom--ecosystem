using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.AiContent;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.AiContent;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class AiContentManager : IAiContentManager
{
	private readonly ISettingRepository _settingRepository;
	private readonly IProductRepository _productRepository;
	private readonly HttpClient _httpClient;
	private readonly string _apiKey;
	private readonly ILogger<AiContentManager> _logger;

	private const string OpenAiApiUrl = "https://api.openai.com/v1/chat/completions";

	public AiContentManager(
		ISettingRepository settingRepository,
		IProductRepository productRepository,
		IConfiguration configuration,
		ILogger<AiContentManager> logger)
	{
		_settingRepository = settingRepository;
		_productRepository = productRepository;
		_logger = logger;
		_apiKey = configuration["OPENAI_API_KEY"] ?? string.Empty;
		_httpClient = new HttpClient();
	}

	public async Task<AiValidationResultDto> ValidateProductNameAsync(long productId, ValidateFieldRequest request)
	{
		var product = _productRepository.Get(productId);
		var context = request.Context ?? new Dictionary<string, string>();
		context["productId"] = productId.ToString();
		context["catalogId"] = product.CatalogId ?? "";

		return await ValidateFieldAsync(
			SettingKey.AI_PROMPT_PRODUCT_NAME_VALIDATION,
			request.Value,
			context);
	}

	public async Task<AiValidationResultDto> ValidateProductDescriptionAsync(long productId, ValidateFieldRequest request)
	{
		var product = _productRepository.Get(productId);
		var context = request.Context ?? new Dictionary<string, string>();
		context["productId"] = productId.ToString();
		context["productName"] = product.Name;
		context["catalogId"] = product.CatalogId ?? "";

		return await ValidateFieldAsync(
			SettingKey.AI_PROMPT_PRODUCT_DESCRIPTION_VALIDATION,
			request.Value,
			context);
	}

	public async Task<AiValidationResultDto> ValidateProductShortDescriptionAsync(long productId, ValidateFieldRequest request)
	{
		var product = _productRepository.Get(productId);
		var context = request.Context ?? new Dictionary<string, string>();
		context["productId"] = productId.ToString();
		context["productName"] = product.Name;
		context["catalogId"] = product.CatalogId ?? "";

		return await ValidateFieldAsync(
			SettingKey.AI_PROMPT_PRODUCT_SHORT_DESCRIPTION_VALIDATION,
			request.Value,
			context);
	}

	public async Task<AiValidationResultDto> ValidateProductMetaAsync(long productId, ValidateMetaRequest request)
	{
		var product = _productRepository.Get(productId);
		var context = request.Context ?? new Dictionary<string, string>();
		context["productId"] = productId.ToString();
		context["productName"] = product.Name;
		context["catalogId"] = product.CatalogId ?? "";

		var combinedValue = $"Meta Title: {request.MetaTitle ?? ""}\nMeta Description: {request.MetaDescription ?? ""}";

		return await ValidateFieldAsync(
			SettingKey.AI_PROMPT_PRODUCT_META_VALIDATION,
			combinedValue,
			context);
	}

	public async Task<AiGeneratedContentDto> GenerateProductDescriptionAsync(long productId, GenerateContentRequest request)
	{
		var product = _productRepository.Get(productId);
		var context = new Dictionary<string, string>
		{
			["productId"] = productId.ToString(),
			["productName"] = product.Name,
			["catalogId"] = product.CatalogId ?? "",
			["existingDescription"] = request.BaseContent ?? product.Description ?? "",
			["style"] = request.Style ?? "professional",
			["maxLength"] = (request.MaxLength ?? 500).ToString(),
			["formatAsHtml"] = request.FormatAsHtml.ToString()
		};

		return await GenerateContentAsync(
			SettingKey.AI_PROMPT_PRODUCT_DESCRIPTION_GENERATE,
			context);
	}

	public async Task<AiGeneratedContentDto> GenerateProductShortDescriptionAsync(long productId, GenerateContentRequest request)
	{
		var product = _productRepository.Get(productId);
		var context = new Dictionary<string, string>
		{
			["productId"] = productId.ToString(),
			["productName"] = product.Name,
			["catalogId"] = product.CatalogId ?? "",
			["existingShortDescription"] = request.BaseContent ?? product.ShortDescription ?? "",
			["style"] = request.Style ?? "professional",
			["maxLength"] = (request.MaxLength ?? 150).ToString()
		};

		return await GenerateContentAsync(
			SettingKey.AI_PROMPT_PRODUCT_SHORT_DESCRIPTION_GENERATE,
			context);
	}

	public async Task<AiGeneratedContentDto> GenerateProductMetaAsync(long productId)
	{
		var product = _productRepository.Get(productId);
		var context = new Dictionary<string, string>
		{
			["productId"] = productId.ToString(),
			["productName"] = product.Name,
			["catalogId"] = product.CatalogId ?? "",
			["description"] = product.Description ?? "",
			["shortDescription"] = product.ShortDescription ?? ""
		};

		return await GenerateContentAsync(
			SettingKey.AI_PROMPT_PRODUCT_META_GENERATE,
			context);
	}

	private async Task<AiValidationResultDto> ValidateFieldAsync(
		SettingKey promptKey,
		string fieldValue,
		Dictionary<string, string>? context = null)
	{
		if (string.IsNullOrWhiteSpace(_apiKey))
		{
			_logger.LogWarning("AI validation skipped - API key not configured");
			return new AiValidationResultDto
			{
				IsValid = true,
				OriginalValue = fieldValue,
				Issues = ["AI validation not configured - API key missing"]
			};
		}

		string? model = null;
		string? systemPrompt = null;
		string? userMessage = null;

		try
		{
			systemPrompt = await _settingRepository.GetValueAsync<string>(promptKey);
			if (string.IsNullOrWhiteSpace(systemPrompt))
			{
				_logger.LogWarning("AI validation skipped - prompt not configured for {PromptKey}", promptKey);
				return new AiValidationResultDto
				{
					IsValid = true,
					OriginalValue = fieldValue,
					Issues = ["AI validation prompt not configured"]
				};
			}

			model = await GetModelName();
			userMessage = BuildUserMessage(fieldValue, context);
			var response = await CallOpenAiAsync(systemPrompt, userMessage, model);

			return ParseValidationResponse(response, fieldValue);
		}
		catch (Exception ex)
		{
			var systemPromptLength = systemPrompt?.Length ?? 0;
			var userMessageLength = userMessage?.Length ?? 0;
			_logger.LogError(ex,
				"AI validation failed: PromptKey={PromptKey}, Model={Model}, " +
				"SystemPromptChars={SystemChars}, UserMessageChars={UserChars}",
				promptKey, model ?? "unknown", systemPromptLength, userMessageLength);
			return new AiValidationResultDto
			{
				IsValid = true,
				OriginalValue = fieldValue,
				Issues = ["AI validation temporarily unavailable"]
			};
		}
	}

	private async Task<AiGeneratedContentDto> GenerateContentAsync(
		SettingKey promptKey,
		Dictionary<string, string> context)
	{
		if (string.IsNullOrWhiteSpace(_apiKey))
		{
			_logger.LogWarning("AI generation skipped - API key not configured");
			return new AiGeneratedContentDto
			{
				Success = false,
				ErrorMessage = "AI generation not configured - API key missing"
			};
		}

		string? model = null;
		string? systemPrompt = null;
		string? userMessage = null;

		try
		{
			systemPrompt = await _settingRepository.GetValueAsync<string>(promptKey);
			if (string.IsNullOrWhiteSpace(systemPrompt))
			{
				_logger.LogWarning("AI generation skipped - prompt not configured for {PromptKey}", promptKey);
				return new AiGeneratedContentDto
				{
					Success = false,
					ErrorMessage = "AI generation prompt not configured"
				};
			}

			model = await GetModelName();
			userMessage = BuildContextMessage(context);
			var response = await CallOpenAiAsync(systemPrompt, userMessage, model);

			return ParseGenerationResponse(response);
		}
		catch (Exception ex)
		{
			var systemPromptLength = systemPrompt?.Length ?? 0;
			var userMessageLength = userMessage?.Length ?? 0;
			_logger.LogError(ex,
				"AI generation failed: PromptKey={PromptKey}, Model={Model}, " +
				"SystemPromptChars={SystemChars}, UserMessageChars={UserChars}",
				promptKey, model ?? "unknown", systemPromptLength, userMessageLength);
			return new AiGeneratedContentDto
			{
				Success = false,
				ErrorMessage = "AI generation temporarily unavailable"
			};
		}
	}

	private async Task<string> GetModelName()
	{
		try
		{
			var model = await _settingRepository.GetValueAsync<string>(SettingKey.AI_MODEL_NAME);
			return string.IsNullOrWhiteSpace(model) ? "gpt-4o" : model;
		}
		catch
		{
			return "gpt-4o";
		}
	}

	private async Task<double> GetTemperature()
	{
		try
		{
			var temp = await _settingRepository.GetValueAsync<string>(SettingKey.AI_TEMPERATURE);
			return double.TryParse(temp, out var result) ? result : 0.3;
		}
		catch
		{
			return 0.3;
		}
	}

	private static string BuildUserMessage(string fieldValue, Dictionary<string, string>? context)
	{
		var message = $"Content to validate:\n\n{fieldValue}";

		if (context != null && context.Count > 0)
		{
			message += "\n\nContext:";
			foreach (var (key, value) in context)
			{
				message += $"\n- {key}: {value}";
			}
		}

		return message;
	}

	private static string BuildContextMessage(Dictionary<string, string> context)
	{
		var message = "Generate content based on the following context:";
		foreach (var (key, value) in context)
		{
			message += $"\n- {key}: {value}";
		}
		return message;
	}

	private async Task<string> CallOpenAiAsync(string systemPrompt, string userMessage, string model)
	{
		var temperature = await GetTemperature();

		// Estimate token count (rough: ~4 chars per token)
		var systemPromptTokens = systemPrompt.Length / 4;
		var userMessageTokens = userMessage.Length / 4;
		var estimatedTotalTokens = systemPromptTokens + userMessageTokens;

		_logger.LogInformation(
			"OpenAI Request: Model={Model}, Temperature={Temperature}, " +
			"SystemPromptChars={SystemChars} (~{SystemTokens} tokens), " +
			"UserMessageChars={UserChars} (~{UserTokens} tokens), " +
			"EstimatedTotalTokens=~{TotalTokens}",
			model, temperature,
			systemPrompt.Length, systemPromptTokens,
			userMessage.Length, userMessageTokens,
			estimatedTotalTokens);

		var request = new
		{
			model,
			messages = new[]
			{
				new { role = "system", content = systemPrompt },
				new { role = "user", content = userMessage }
			},
			temperature,
			response_format = new { type = "json_object" }
		};

		_httpClient.DefaultRequestHeaders.Authorization =
			new AuthenticationHeaderValue("Bearer", _apiKey);

		var response = await _httpClient.PostAsJsonAsync(OpenAiApiUrl, request);

		if (!response.IsSuccessStatusCode)
		{
			var errorBody = await response.Content.ReadAsStringAsync();
			_logger.LogError(
				"OpenAI API Error: StatusCode={StatusCode}, Model={Model}, " +
				"EstimatedTokens=~{Tokens}, Response={Response}",
				(int)response.StatusCode, model, estimatedTotalTokens, errorBody);
			response.EnsureSuccessStatusCode();
		}

		var result = await response.Content.ReadFromJsonAsync<OpenAiResponse>();
		_logger.LogInformation("OpenAI Request successful for Model={Model}", model);
		return result?.Choices?.FirstOrDefault()?.Message?.Content ?? "{}";
	}

	private static AiValidationResultDto ParseValidationResponse(string jsonResponse, string originalValue)
	{
		try
		{
			using var doc = JsonDocument.Parse(jsonResponse);
			var root = doc.RootElement;

			return new AiValidationResultDto
			{
				IsValid = root.TryGetProperty("isValid", out var isValid) && isValid.GetBoolean(),
				OriginalValue = originalValue,
				SuggestedValue = root.TryGetProperty("suggestedValue", out var suggested)
					? suggested.ValueKind == JsonValueKind.Null ? null : suggested.GetString()
					: null,
				Issues = ParseStringArray(root, "issues"),
				Suggestions = ParseStringArray(root, "suggestions"),
				ConfidenceScore = root.TryGetProperty("confidenceScore", out var score)
					? score.GetDouble()
					: 0.0
			};
		}
		catch
		{
			return new AiValidationResultDto
			{
				IsValid = true,
				OriginalValue = originalValue,
				Issues = ["Failed to parse AI response"]
			};
		}
	}

	private static AiGeneratedContentDto ParseGenerationResponse(string jsonResponse)
	{
		try
		{
			using var doc = JsonDocument.Parse(jsonResponse);
			var root = doc.RootElement;

			return new AiGeneratedContentDto
			{
				Success = root.TryGetProperty("success", out var success) && success.GetBoolean(),
				GeneratedContent = root.TryGetProperty("generatedContent", out var content)
					? content.GetString()
					: null,
				HtmlContent = root.TryGetProperty("htmlContent", out var html)
					? html.GetString()
					: null,
				AlternativeContent = root.TryGetProperty("alternativeContent", out var alt)
					? alt.GetString()
					: null,
				ErrorMessage = root.TryGetProperty("errorMessage", out var error)
					? error.GetString()
					: null
			};
		}
		catch
		{
			return new AiGeneratedContentDto
			{
				Success = false,
				ErrorMessage = "Failed to parse AI response"
			};
		}
	}

	private static List<string> ParseStringArray(JsonElement root, string propertyName)
	{
		if (!root.TryGetProperty(propertyName, out var arrayElement) ||
			arrayElement.ValueKind != JsonValueKind.Array)
		{
			return [];
		}

		return arrayElement.EnumerateArray()
			.Where(e => e.ValueKind == JsonValueKind.String)
			.Select(e => e.GetString() ?? "")
			.Where(s => !string.IsNullOrEmpty(s))
			.ToList();
	}

	private class OpenAiResponse
	{
		public List<Choice>? Choices { get; set; }
	}

	private class Choice
	{
		public Message? Message { get; set; }
	}

	private class Message
	{
		public string? Content { get; set; }
	}
}
