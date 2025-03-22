using System.Net.Http.Json;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Common.Domain.Managers;

public class OfficeServerApiManager : IOfficeServerApiManager
{
	private readonly HttpClient _httpClient = new();

	public OfficeServerApiManager()
	{
		_httpClient.BaseAddress = new Uri(LegacyConstants.OfficeServerApiUrl);
	}

	public Task SmsQueueAsync(SMSQueueRequest request) =>
		_httpClient.PostAsJsonAsync($"/SMS/Queue", request);
}
