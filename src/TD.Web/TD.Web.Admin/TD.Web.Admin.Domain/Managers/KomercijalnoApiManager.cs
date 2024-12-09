using TD.Web.Admin.Contracts.Requests.KomercijalnoApi;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Web.Admin.Contracts;
using TD.Web.Common.Domain;
using System.Net.Http.Json;

namespace TD.Web.Admin.Domain.Managers;

public class KomercijalnoApiManager : IKomercijalnoApiManager
{
    private readonly HttpClient _httpClient = new ();
    public KomercijalnoApiManager()
    {
        _httpClient.BaseAddress = new Uri(string.Format(Constants.KomercijalnoApiUrlFormat, DateTime.Now.Year));
    }

    public async Task<DokumentDto> DokumentiPostAsync(KomercijalnoApiDokumentiCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"/dokumenti", request);
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<DokumentDto>())!;
    }

    public async Task<StavkaDto> StavkePostAsync(StavkaCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"/stavke", request);
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<StavkaDto>())!;
    }

    public async Task<KomentarDto> DokumentiKomentariPostAsync(CreateKomentarRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"/komentari", request);
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<KomentarDto>())!;
    }

    public async Task StavkeDeleteAsync(StavkeDeleteRequest request)
    {
        var response = await _httpClient.DeleteAsync(
            $"/stavke?VrDok={request.VrDok}&BrDok={request.BrDok}");
        response.HandleStatusCode();
    }

    public async Task FlushCommentsAsync(FlushCommentsRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"/komentari/flush", request);
        response.HandleStatusCode();
    }
}