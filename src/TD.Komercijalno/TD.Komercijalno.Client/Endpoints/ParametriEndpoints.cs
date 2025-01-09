using System.Net.Http.Json;
using TD.Komercijalno.Contracts.Requests.Parametri;

namespace TD.Komercijalno.Client.Endpoints;

public class ParametriEndpoints(Func<HttpClient> client)
{
    public async Task UpdateAsync(UpdateParametarRequest request)
    {
        var response = await client().PutAsJsonAsync("parametri", request);
        TDKomercijalnoClient.HandleStatusCode(response);
    }
}