using System.Net.Http.Json;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Client.Endpoints;

public class DokumentiEndpoints(
    Func<HttpClient> client,
    Action<HttpResponseMessage> handleStatusCode)
{
    public async Task<DokumentDto> CreateAsync(DokumentCreateRequest request)
    {
        var response = await client().PostAsJsonAsync("dokumenti", request);
        handleStatusCode(response);
        return (await response.Content.ReadFromJsonAsync<DokumentDto>())!;
    }

    public async Task UpdateDokOut(DokumentSetDokOutRequest request)
    {
        var response = await client().PostAsJsonAsync($"dokumenti/{request.VrDok}/{request.BrDok}/dok-out", request);
        handleStatusCode(response);
    }
}