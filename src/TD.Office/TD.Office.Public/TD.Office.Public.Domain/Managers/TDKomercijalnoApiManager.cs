using System.Net.Http.Json;
using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Responses;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Dtos.Mesto;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Enums.ValidationCodes;
using TD.Office.Common.Contracts.IManagers;
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
    private readonly ISettingManager _settingManager;

    public TDKomercijalnoApiManager(
        ILogger<TDKomercijalnoApiManager> logger,
        LSCoreContextUser contextUser,
        IUserManager userManager,
        ISettingManager settingManager
    )
        : base(logger, contextUser)
    {
        _userManager = userManager;
        _settingManager = settingManager;
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
        request.RefId = 107;
        request.ZapId = 107;

        var apisToCreateInto = _settingManager
            .Queryable()
            .Where(x => x.Key == SettingKeys.OTVARANJE_PARTNERA_BAZA.ToString())
            .ToList();

        if (apisToCreateInto.Count == 0)
            throw new LSCoreNotFoundException();

        #region Check if there is a partner with the same duplicate data in any of the APIs
        foreach (var api in apisToCreateInto)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(api.Value);
            var response = await client.GetAsync(
                $"/partneri-duplikat?pib={request.Pib}&mbroj={request.Mbroj}"
            );
            response.HandleStatusCode();
            var isDuplikat = await response.Content.ReadFromJsonAsync<bool>();

            if (isDuplikat)
                throw new LSCoreBadRequestException(
                    $"Partner sa istim PIB/MB već postoji u bazi {api.Value}!"
                );
        }
        #endregion

        #region Check if all databases have same last PPID
        var lastIds = new Dictionary<string, int>();

        foreach (var api in apisToCreateInto)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(api.Value);
            var response = await client.GetAsync("/partneri-poslednji-id");
            response.HandleStatusCode();
            var id = (await response.Content.ReadFromJsonAsync<int>())!;
            lastIds.Add(api.Value, id);
        }

        if (lastIds.Values.Distinct().Count() != 1)
            throw new LSCoreBadRequestException(
                $"Baze nemaju isti poslednji ID partnera! {string.Join(", ", lastIds)}"
            );
        #endregion

        #region Create partner in all APIs
        var newId = -1;
        foreach (var api in apisToCreateInto)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(api.Value);
            var response = await client.PostAsJsonAsync("/partneri", request);
            response.HandleStatusCode();
            if (newId == -1)
                newId = (await response.Content.ReadFromJsonAsync<int>())!;
        }
        #endregion

        if (newId == -1)
            throw new LSCoreInternalException();

        return newId;
    }

    public async Task<List<MestoDto>> GetPartnersMestaAsync()
    {
        var response = await _httpClient.GetAsync("/mesta");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<MestoDto>>())!;
    }

    public async Task<List<PPKategorija>> GetPartnersKategorijeAsync()
    {
        var response = await _httpClient.GetAsync("/partneri-kategorije");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<PPKategorija>>())!;
    }
}
