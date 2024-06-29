using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.OfficeServer.Contracts.Requests.SMS;
using System.Net.Http.Json;

namespace TD.Web.Common.Domain.Managers;

public class OfficeServerApiManager : IOfficeServerApiManager
{
    private readonly HttpClient _httpClient = new();
    public OfficeServerApiManager()
    {
        _httpClient.BaseAddress = new Uri(Contracts.Constants.OfficeServerApiUrl);
    }

    public Task SmsQueueAsync(SMSQueueRequest request) =>
        _httpClient.PostAsJsonAsync($"/SMS/Queue", request);
}