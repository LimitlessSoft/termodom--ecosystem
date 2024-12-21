using System.Net.Http.Json;
using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.IstorijaUplata;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Dtos.Mesto;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.Promene;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.IstorijaUplata;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Promene;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
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
    private readonly ILogManager _logManager;
    private readonly IConfigurationRoot _configurationRoot;

    public TDKomercijalnoApiManager(
        ILogger<TDKomercijalnoApiManager> logger,
        LSCoreContextUser contextUser,
        IUserManager userManager,
        ISettingManager settingManager,
        ILogManager logManager,
        IConfigurationRoot configurationRoot
    )
        : base(logger, contextUser)
    {
        _userManager = userManager;
        _settingManager = settingManager;
        _logManager = logManager;
        _configurationRoot = configurationRoot;
        SetYear(DateTime.UtcNow.Year);
    }

    /// <summary>
    /// Changes the base address of the HttpClient to the Komercijalno API URL for the given year.
    /// Do not call this method directly from Injected instance of ITDKomercijalnoApiManager but use the factory if you need to access the API for a different year.
    /// </summary>
    /// <param name="year"></param>
    public void SetYear(int year)
    {
        var envPostpend = _configurationRoot["DEPLOY_ENV"];
        if (envPostpend is "production" or "release")
            envPostpend = null;
        else
            envPostpend = "-" + envPostpend;

        _httpClient.BaseAddress = new Uri(
            string.Format(Constants.KomercijalnoApiUrlFormat, year, envPostpend)
        );
    }

    public async Task<int> GetPartnersCountAsync()
    {
        var response = await _httpClient.GetAsync($"/partnerti-count");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<int>())!;
    }

    public async Task<List<VrstaDokDto>> GetMultipleVrDokAsync()
    {
        var response = await _httpClient.GetAsync($"/vrste-dokumenata");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<VrstaDokDto>>())!;
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

    public async Task<DokumentDto> DokumentiPostAsync(DokumentCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"/dokumenti", request);
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<DokumentDto>())!;
    }

    public async Task<StavkaDto> StavkePostAsync(StavkaCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"/stavke", request);
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<StavkaDto>())!;
    }

    public async Task<double> GetProdajnaCenaNaDanAsync(
        ProceduraGetProdajnaCenaNaDanRequest request
    )
    {
        var response = await _httpClient.GetAsync(
            $"/procedure/prodajna-cena-na-dan?magacinId={request.MagacinId}&datum={request.Datum:yyyy-MM-ddT00:00:00.000Z}&robaId={request.RobaId}"
        );
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<double>())!;
    }

    public async Task<List<ProdajnaCenaNaDanDto>> GetProdajnaCenaNaDanOptimizedAsync(
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

    public async Task<List<DokumentDto>> GetMultipleDokumentAsync(
        DokumentGetMultipleRequest request
    )
    {
        var queryParameters = new List<string>();

        if (request.VrDok != null && request.VrDok.Any())
            queryParameters.Add("vrDok=" + string.Join("&vrDok=", request.VrDok));

        if (request.IntBroj != null)
            queryParameters.Add("intBroj=" + request.IntBroj);

        if (request.KodDok.HasValue)
            queryParameters.Add("kodDok=" + request.KodDok.Value);

        if (request.Flag != null)
            queryParameters.Add("flag=" + request.Flag);

        if (request.DatumOd != null)
            queryParameters.Add(
                "datumOd=" + request.DatumOd.Value.ToString("yyyy-MM-ddT00:00:00.000Z")
            );

        if (request.DatumDo != null)
            queryParameters.Add(
                "datumDo=" + request.DatumDo.Value.ToString("yyyy-MM-ddT00:00:00.000Z")
            );

        if (request.Linked != null)
            queryParameters.Add("linked=" + request.Linked);

        if (request.MagacinId != null)
            queryParameters.Add("magacinId=" + request.MagacinId);

        if (request.PPID != null && request.PPID.Any())
            queryParameters.Add("ppid=" + string.Join("&ppid=", request.PPID));

        if (request.NUID != null && request.NUID.Any())
            queryParameters.Add("nuid=" + string.Join("&nuid=", request.NUID));

        var response = await _httpClient.GetAsync(
            $"/dokumenti?{string.Join("&", queryParameters)}"
        );
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<DokumentDto>>())!;
    }

    public async Task<LSCoreSortedAndPagedResponse<PartnerDto>> GetPartnersAsync(
        PartneriGetMultipleRequest request
    )
    {
        var response = await _httpClient.GetAsync(
            $"/partneri?aktivan={request.Aktivan}&sortDirection={request.SortDirection}&currentPage={request.CurrentPage}&pageSize={request.PageSize}&searchKeyword={request.SearchKeyword}{(request.Ppid == null ? "" : "&ppid=" + string.Join("&ppid=", request.Ppid))}"
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

        var mobilniPermission = _userManager.HasPermission(Permission.PartneriVidiMobilni);

        var payload = res.Payload!.Select(x => new PartnerDto
            {
                Ppid = x.Ppid,
                Naziv = x.Naziv,
                Adresa = x.Adresa,
                Posta = x.Posta,
                Pib = x.Pib,
                Mobilni = mobilniPermission
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

    public async Task<PartnerDto> GetPartnerAsync(LSCoreIdRequest request)
    {
        var response = await _httpClient.GetAsync($"/partneri/{request.Id}");
        response.HandleStatusCode();
        var p =
            await response.Content.ReadFromJsonAsync<Komercijalno.Contracts.Dtos.Partneri.PartnerDto>();
        return new PartnerDto
        {
            Ppid = p.Ppid,
            Naziv = p.Naziv,
            Adresa = p.Adresa,
            Posta = p.Posta,
            Pib = p.Pib,
            Mobilni = _userManager.HasPermission(Permission.PartneriVidiMobilni)
                ? p.Mobilni
                : CommonValidationCodes.CMN_001.GetDescription(),
        };
    }

    public async Task<int> CreatePartnerAsync(PartneriCreateRequest request)
    {
        request.RefId = 107;
        request.ZapId = 107;

        var apisToCreateInto = _settingManager
            .Queryable()
            .Where(x => x.Key == SettingKey.OTVARANJE_PARTNERA_BAZA.ToString())
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

        _logManager.Log(LogKey.NoviKomercijalnoPartner, newId.ToString());

        return newId;
    }

    public async Task<List<MestoDto>> GetPartnersMestaAsync()
    {
        var response = await _httpClient.GetAsync("/mesta");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<MestoDto>>())!
            .OrderBy(x => x.Naziv)
            .ToList();
    }

    public async Task<List<PPKategorija>> GetPartnersKategorijeAsync()
    {
        var response = await _httpClient.GetAsync("/partneri-kategorije");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<PPKategorija>>())!;
    }

    public async Task<List<NacinPlacanjaDto>> GetMultipleNaciniPlacanjaAsync()
    {
        var response = await _httpClient.GetAsync("/nacini-placanja");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<NacinPlacanjaDto>>())!;
    }

    public async Task<List<RobaDto>> GetMultipleRobaAsync(RobaGetMultipleRequest request)
    {
        var response = await _httpClient.GetAsync($"/roba?vrsta={request.Vrsta}");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<List<RobaDto>>())!;
    }

    public async Task CreateStavkaAsync(StavkaCreateRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"/stavke", request);
        response.HandleStatusCode();
    }

    public async Task SetDokumentNacinPlacanjaAsync(
        DokumentSetNacinPlacanjaRequest dokumentSetNacinPlacanjaRequest
    )
    {
        var response = await _httpClient.PutAsync(
            $"/dokumenti/{dokumentSetNacinPlacanjaRequest.VrDok}/{dokumentSetNacinPlacanjaRequest.BrDok}/nacin-placanja/{dokumentSetNacinPlacanjaRequest.NUID}",
            null
        );
        response.HandleStatusCode();
    }

    public async Task<RobaDto> GetRobaAsync(LSCoreIdRequest lsCoreIdRequest)
    {
        var response = await _httpClient.GetAsync($"/roba/{lsCoreIdRequest.Id}");
        response.HandleStatusCode();
        return (await response.Content.ReadFromJsonAsync<RobaDto>())!;
    }

    public async Task<List<IstorijaUplataDto>> GetMultipleIstorijaUplataAsync(
        IstorijaUplataGetMultipleRequest request
    )
    {
        var queryParams =
            request.PPID != null
                ? string.Join("&", request.PPID.Select(p => $"ppid={p}"))
                : string.Empty;

        var response = await _httpClient.GetAsync($"/istorija-uplata?{queryParams}");
        response.HandleStatusCode();
        return await response.Content.ReadFromJsonAsync<List<IstorijaUplataDto>>();
    }

    public async Task<List<IzvodDto>> GetMultipleIzvodAsync(IzvodGetMultipleRequest request)
    {
        var queryParameters = new List<string>();

        if (request.PPID != null && request.PPID.Any())
            queryParameters.Add("ppid=" + string.Join("&ppid=", request.PPID));

        if (request.PozivNaBroj != null)
            queryParameters.Add("pozivNaBroj=" + request.PozivNaBroj);

        var response = await _httpClient.GetAsync($"/izvodi?{String.Join("&", queryParameters)}");
        response.HandleStatusCode();
        return await response.Content.ReadFromJsonAsync<List<IzvodDto>>();
    }

    public async Task<List<PromenaDto>> GetMultiplePromeneAsync(PromenaGetMultipleRequest request)
    {
        var queryParameters = new List<string>();

        if (!string.IsNullOrEmpty(request.KontoStartsWith))
            queryParameters.Add("KontoStartsWith=" + request.KontoStartsWith);

        if (request.PPID != null && request.PPID.Any())
            queryParameters.Add("ppid=" + string.Join("&ppid=", request.PPID));

        var queryString = string.Join("&", queryParameters);

        var response = await _httpClient.GetAsync($"/promene?{queryString}");
        response.HandleStatusCode();
        return await response.Content.ReadFromJsonAsync<List<PromenaDto>>();
    }
}
