using System.Net.Http.Json;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Client.Endpoints;

public class StavkeEndpoints(
    Func<HttpClient> client,
    Action<HttpResponseMessage> handleStatusCode)
{
    public async Task<StavkaDto> CreateAsync(StavkaCreateRequest request)
    {
        var response = await client().PostAsJsonAsync("stavke", request);
        handleStatusCode(response);
        return (await response.Content.ReadFromJsonAsync<StavkaDto>())!;
    }
}