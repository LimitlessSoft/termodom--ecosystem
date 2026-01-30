using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Settings;
using TD.Web.Admin.Contracts.Requests.Settings;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuth]
[ApiController]
public class SettingsController(ISettingRepository settingRepository) : ControllerBase
{
	[HttpGet]
	[Route("/settings/ai")]
	public AiSettingsDto GetAiSettings()
	{
		return new AiSettingsDto
		{
			ModelName = GetSettingValue(SettingKey.AI_MODEL_NAME),
			MaxTokens = GetSettingValue(SettingKey.AI_MAX_TOKENS),
			Temperature = GetSettingValue(SettingKey.AI_TEMPERATURE),

			ProductNameValidation = GetSettingValue(SettingKey.AI_PROMPT_PRODUCT_NAME_VALIDATION),
			ProductDescriptionValidation = GetSettingValue(SettingKey.AI_PROMPT_PRODUCT_DESCRIPTION_VALIDATION),
			ProductShortDescriptionValidation = GetSettingValue(SettingKey.AI_PROMPT_PRODUCT_SHORT_DESCRIPTION_VALIDATION),
			ProductMetaValidation = GetSettingValue(SettingKey.AI_PROMPT_PRODUCT_META_VALIDATION),

			ProductDescriptionGenerate = GetSettingValue(SettingKey.AI_PROMPT_PRODUCT_DESCRIPTION_GENERATE),
			ProductShortDescriptionGenerate = GetSettingValue(SettingKey.AI_PROMPT_PRODUCT_SHORT_DESCRIPTION_GENERATE),
			ProductMetaGenerate = GetSettingValue(SettingKey.AI_PROMPT_PRODUCT_META_GENERATE),

			BlogTitleValidation = GetSettingValue(SettingKey.AI_PROMPT_BLOG_TITLE_VALIDATION),
			BlogContentValidation = GetSettingValue(SettingKey.AI_PROMPT_BLOG_CONTENT_VALIDATION),
			BlogContentGenerate = GetSettingValue(SettingKey.AI_PROMPT_BLOG_CONTENT_GENERATE),
		};
	}

	[HttpPut]
	[Route("/settings/ai")]
	public IActionResult SaveAiSettings([FromBody] SaveAiSettingsRequest request)
	{
		SetSettingIfNotNull(SettingKey.AI_MODEL_NAME, request.ModelName);
		SetSettingIfNotNull(SettingKey.AI_MAX_TOKENS, request.MaxTokens);
		SetSettingIfNotNull(SettingKey.AI_TEMPERATURE, request.Temperature);

		SetSettingIfNotNull(SettingKey.AI_PROMPT_PRODUCT_NAME_VALIDATION, request.ProductNameValidation);
		SetSettingIfNotNull(SettingKey.AI_PROMPT_PRODUCT_DESCRIPTION_VALIDATION, request.ProductDescriptionValidation);
		SetSettingIfNotNull(SettingKey.AI_PROMPT_PRODUCT_SHORT_DESCRIPTION_VALIDATION, request.ProductShortDescriptionValidation);
		SetSettingIfNotNull(SettingKey.AI_PROMPT_PRODUCT_META_VALIDATION, request.ProductMetaValidation);

		SetSettingIfNotNull(SettingKey.AI_PROMPT_PRODUCT_DESCRIPTION_GENERATE, request.ProductDescriptionGenerate);
		SetSettingIfNotNull(SettingKey.AI_PROMPT_PRODUCT_SHORT_DESCRIPTION_GENERATE, request.ProductShortDescriptionGenerate);
		SetSettingIfNotNull(SettingKey.AI_PROMPT_PRODUCT_META_GENERATE, request.ProductMetaGenerate);

		SetSettingIfNotNull(SettingKey.AI_PROMPT_BLOG_TITLE_VALIDATION, request.BlogTitleValidation);
		SetSettingIfNotNull(SettingKey.AI_PROMPT_BLOG_CONTENT_VALIDATION, request.BlogContentValidation);
		SetSettingIfNotNull(SettingKey.AI_PROMPT_BLOG_CONTENT_GENERATE, request.BlogContentGenerate);

		return Ok();
	}

	private string? GetSettingValue(SettingKey key)
	{
		var setting = settingRepository.GetSettingOrDefault(key);
		return setting?.Value;
	}

	private void SetSettingIfNotNull(SettingKey key, string? value)
	{
		if (value != null)
			settingRepository.SetOrCreateValue(key, value);
	}
}
