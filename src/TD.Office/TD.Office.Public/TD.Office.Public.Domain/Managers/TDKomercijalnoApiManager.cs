using TD.Office.Public.Contracts.Requests.KomercijalnoApi;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Office.Common.Domain.Extensions;
using Microsoft.Extensions.Logging;
using TD.Office.Public.Contracts;
using System.Net.Http.Json;
using TD.Office.Public.Contracts.Dtos.Partners;

namespace TD.Office.Public.Domain.Managers;

public class TDKomercijalnoApiManager : ITDKomercijalnoApiManager
{
    private readonly HttpClient _httpClient = new ();
    public TDKomercijalnoApiManager(ILogger<TDKomercijalnoApiManager> logger)
    {
        _httpClient.BaseAddress = new Uri(string.Format(Constants.KomercijalnoApiUrlFormat, DateTime.Now.Year));
    }

    public async Task<List<RobaUMagacinuGetDto>> GetRobaUMagacinuAsync(KomercijalnoApiGetRobaUMagacinuRequest request)
    {
        var response = await _httpClient.GetAsync($"/roba-u-magacinu?magacinId={request.MagacinId}");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<RobaUMagacinuGetDto>>())!;
    }

    public async Task<List<NabavnaCenaNaDanDto>> GetNabavnaCenaNaDanAsync(ProceduraGetNabavnaCenaNaDanRequest request)
    {
        var response =
            await _httpClient.GetAsync(
                $"/procedure/nabavna-cena-na-dan?datum={request.Datum:yyyy-MM-ddT00:00:00.000Z}");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<NabavnaCenaNaDanDto>>())!;
    }

    public async Task<List<ProdajnaCenaNaDanDto>> GetProdajnaCenaNaDanAsync(
        ProceduraGetProdajnaCenaNaDanOptimizedRequest request)
    {
        var response = await _httpClient.GetAsync(
            $"/procedure/prodajna-cena-na-dan-optimized?magacinId={request.MagacinId}&datum={request.Datum:yyyy-MM-ddT00:00:00.000Z}");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<ProdajnaCenaNaDanDto>>())!;
    }

    public async Task<List<MagacinDto>> GetMagaciniAsync()
    {
        var response = await _httpClient.GetAsync("/magacini");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<MagacinDto>>())!;
    }

    public async Task<DokumentDto> GetDokumentAsync(DokumentGetRequest request)
    {
        var response = await _httpClient.GetAsync($"/dokumenti/{request.VrDok}/{request.BrDok}");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<DokumentDto>())!;
    }

    public async Task<List<PartnerDto>> GetPartnersAsync()
    {
        var response = await _httpClient.GetAsync("/partneri");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<PartnerDto>>())!;
    }
}