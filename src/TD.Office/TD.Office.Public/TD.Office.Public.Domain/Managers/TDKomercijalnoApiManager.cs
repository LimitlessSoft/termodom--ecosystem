using System.Net.Http.Json;
using LSCore.Contracts;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Responses;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Enums.ValidationCodes;
using TD.Office.Common.Domain.Extensions;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Dtos.Partners;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;

namespace TD.Office.Public.Domain.Managers;

public class TDKomercijalnoApiManager
    : LSCoreManagerBase<TDKomercijalnoApiManager>,
        ITDKomercijalnoApiManager
{
    private readonly HttpClient _httpClient = new();
    private readonly IUserManager _userManager;

    public TDKomercijalnoApiManager(
        ILogger<TDKomercijalnoApiManager> logger,
        LSCoreContextUser contextUser,
        IUserManager userManager
    )
        : base(logger, contextUser)
    {
        _userManager = userManager;
        _httpClient.BaseAddress = new Uri(
            string.Format(Constants.KomercijalnoApiUrlFormat, DateTime.Now.Year)
        );
    }

    public async Task<List<RobaUMagacinuGetDto>> GetRobaUMagacinuAsync(
        KomercijalnoApiGetRobaUMagacinuRequest request
    )
    {
        var response = await _httpClient.GetAsync(
            $"/roba-u-magacinu?magacinId={request.MagacinId}"
        );
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<RobaUMagacinuGetDto>>())!;
    }

    public async Task<List<NabavnaCenaNaDanDto>> GetNabavnaCenaNaDanAsync(
        ProceduraGetNabavnaCenaNaDanRequest request
    )
    {
        var response = await _httpClient.GetAsync(
            $"/procedure/nabavna-cena-na-dan?datum={request.Datum:yyyy-MM-ddT00:00:00.000Z}"
        );
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<NabavnaCenaNaDanDto>>())!;
    }

    public async Task<List<ProdajnaCenaNaDanDto>> GetProdajnaCenaNaDanAsync(
        ProceduraGetProdajnaCenaNaDanOptimizedRequest request
    )
    {
        var response = await _httpClient.GetAsync(
            $"/procedure/prodajna-cena-na-dan-optimized?magacinId={request.MagacinId}&datum={request.Datum:yyyy-MM-ddT00:00:00.000Z}"
        );
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

    public async Task<LSCoreSortedAndPagedResponse<PartnerDto>> GetPartnersAsync(
        PartneriGetMultipleRequest request
    )
    {
        var response = await _httpClient.GetAsync(
            $"/partneri?currentPage={request.CurrentPage}&pageSize={request.PageSize}&searchKeyword={request.SearchKeyword}"
        );
        response.HandleStatusCode();
        var res = (
            await response.Content.ReadFromJsonAsync<
                LSCoreSortedAndPagedResponse<Komercijalno.Contracts.Dtos.Partneri.PartnerDto>
            >()
        )!;

        var pag = new LSCoreSortedAndPagedResponse<PartnerDto>.PaginationData(
            res.Pagination!.Page,
            res.Pagination.PageSize,
            res.Pagination.TotalCount
        );
        var payload = res.Payload!.Select(x => new PartnerDto
            {
                Ppid = x.Ppid,
                Naziv = x.Naziv,
                Adresa = x.Adresa,
                Posta = x.Posta,
                Pib = x.Pib,
                Mobilni = _userManager.HasPermission(Permission.PartneriVidiMobilni)
                    ? x.Mobilni
                    : CommonValidationCodes.CMN_001.GetDescription(),
            })
            .ToList();
        return new LSCoreSortedAndPagedResponse<PartnerDto>
        {
            Pagination = pag,
            Payload = payload,
        };
    }

    public async Task<int> CreatePartnerAsync(PartneriCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("/partneri", request);
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<int>())!;
    }
}
