using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using LSCore.Exceptions;
using TD.Office.MassSMS.Contracts.Dtos;
using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Client;

public class MassSMSApiClient : LSCoreApiClient
{
	new HttpClient _httpClient { get; }

	public MassSMSApiClient(LSCoreApiClientRestConfiguration<MassSMSApiClient> configuration)
		: base(configuration)
	{
		var handler = new HttpClientHandler();
		handler.ServerCertificateCustomValidationCallback = (
			sender,
			cert,
			chain,
			sslPolicyErrors
		) => true;
		_httpClient = new HttpClient(handler);
		_httpClient.BaseAddress = new Uri(configuration.BaseUrl);
		if (string.IsNullOrWhiteSpace(configuration.LSCoreApiKey))
			return;
		_httpClient.DefaultRequestHeaders.Add("X-LS-Key", configuration.LSCoreApiKey);
	}

	public async Task InvokeSendingAsync() =>
		HandleStatusCode(await _httpClient.PostAsync("/mass-sms/invoke-sending", null));

	public async Task<string> GetCurrentStatusAsync() =>
		await _httpClient.GetStringAsync("/mass-sms/status");

	public async Task<int> GetQueueCountAsync() =>
		await _httpClient.GetFromJsonAsync<int>("/mass-sms/queue-count");

	public async Task<List<SMSDto>> GetQueueAsync() =>
		await _httpClient.GetFromJsonAsync<List<SMSDto>>("/mass-sms/queue");

	public async Task ClearQueueAsync() =>
		HandleStatusCode(await _httpClient.DeleteAsync("/mass-sms/clear-queue"));

	public async Task MassQueueAsync(MassQueueSmsRequest massQueueSmsRequest) =>
		HandleStatusCode(
			await _httpClient.PostAsJsonAsync("/mass-sms/mass-queue", massQueueSmsRequest)
		);

	public async Task ClearDuplicatesAsync() =>
		HandleStatusCode(await _httpClient.DeleteAsync("/mass-sms/clear-duplicates"));

	public async Task SetTextAsync(SetTextRequest setTextRequest) =>
		HandleStatusCode(await _httpClient.PutAsJsonAsync("/mass-sms/text", setTextRequest));

	public async Task<bool> IsBlacklistedAsync(string number)
	{
		var response = await _httpClient.GetAsync($"/mass-sms/{number}/is-blacklisted");
		if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			return false;
		if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		if (response.StatusCode != System.Net.HttpStatusCode.OK)
			throw new Exception(
				$"Error while checking if number {number} is blacklisted. Status code: {response.StatusCode}"
			);
		HandleStatusCode(response);
		return await response.Content.ReadFromJsonAsync<bool>();
	}

	public async Task Blacklist(string number)
	{
		var response = await _httpClient.PostAsync($"/mass-sms/{number}/blacklist", null);
		if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
			throw new LSCoreNotFoundException();
		if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
			throw new LSCoreBadRequestException(await response.Content.ReadAsStringAsync());
		if (response.StatusCode != System.Net.HttpStatusCode.OK)
			throw new Exception(
				$"Error while checking if number {number} is blacklisted. Status code: {response.StatusCode}"
			);
		HandleStatusCode(response);
	}

	public async Task ClearBlacklistedAsync() =>
		HandleStatusCode(await _httpClient.DeleteAsync("/mass-sms/clear-blacklisted"));
}
