using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.Komercijalno.Contracts.Requests.Izvodi;

namespace TD.Komercijalno.Client.Endpoints;

public class IzvodiEndpoints(
    Func<HttpClient> client,
    Action<HttpResponseMessage> handleStatusCode
)
{
    public async Task<List<IzvodDto>> GetMultipleAsync(IzvodGetMultipleRequest request)
    {
        var response = await client().GetAsJsonAsync("izvodi", request);
        handleStatusCode(response);
        return (await response.Content.ReadFromJsonAsync<List<IzvodDto>>())!;
    }
}