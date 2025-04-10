using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.OfficeServer.Contracts.Requests.SMS;

namespace TD.OfficeServer.Client;

public class TDOfficeServerClient(
	LSCoreApiClientRestConfiguration<TDOfficeServerClient> configuration
) : LSCoreApiClient(configuration)
{
	public async Task SendSMSAsync(SMSQueueRequest request) =>
		await _httpClient.PostAsJsonAsync($"/SMS/Queue", request);
}
