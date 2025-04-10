using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.Office.MassSMS.Contracts.Dtos;
using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Client;

public class MassSMSApiClient(LSCoreApiClientRestConfiguration<MassSMSApiClient> configuration)
	: LSCoreApiClient(configuration)
{
	public async Task<string> GetCurrentStatusAsync() =>
		await _httpClient.GetStringAsync("/mass-sms/status");

	public async Task<int> GetQueueCountAsync() =>
		await _httpClient.GetFromJsonAsync<int>("/mass-sms/queue-count");

	public async Task<List<SMSDto>> GetQueueAsync() =>
		await _httpClient.GetFromJsonAsync<List<SMSDto>>("/mass-sms/queue");

	public async Task ClearQueueAsync() => await _httpClient.DeleteAsync("/mass-sms/clear-queue");

	public async Task MassQueueAsync(MassQueueSmsRequest massQueueSmsRequest) =>
		await _httpClient.PostAsJsonAsync("/mass-sms/mass-queue", massQueueSmsRequest);
}
