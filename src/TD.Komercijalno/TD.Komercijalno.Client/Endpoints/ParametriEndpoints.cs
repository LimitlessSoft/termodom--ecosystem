using System.Net.Http.Json;
using LSCore.ApiClient.Rest;
using TD.Komercijalno.Contracts.Requests.Parametri;

namespace TD.Komercijalno.Client.Endpoints;

public class ParametriEndpoints(
    Func<HttpClient> client,
    Action<HttpResponseMessage> handleStatusCode
)
{
    public async Task UpdateAsync(UpdateParametarRequest request)
    {
        var response = await client().PutAsJsonAsync("parametri", request);
        handleStatusCode(response);
    }
}
