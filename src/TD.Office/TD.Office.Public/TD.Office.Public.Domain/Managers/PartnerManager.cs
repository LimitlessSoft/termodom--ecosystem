using LSCore.Contracts;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Partners;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Requests.Partneri;
using LSCore.Contracts.Responses;
using TD.Office.Common.Domain.Extensions;
using System.Net.Http.Json;
using System.Linq;
using TD.Komercijalno.Contracts.Enums;

namespace TD.Office.Public.Domain.Managers;

public class PartnerManager : LSCoreManagerBase<PartnerManager>, IPartnerManager
{
    private readonly HttpClient _httpClient = new();
    private readonly ILogger<PartnerManager> _logger;
    private readonly OfficeDbContext _dbContext;
    private readonly LSCoreContextUser _currentUser;
    private readonly ILogManager _logManager;
    private readonly ISettingManager _settingManager;
    private readonly ITDKomercijalnoApiManager _komercijalnoApiManager;

    public PartnerManager(
        ILogger<PartnerManager> logger,
        OfficeDbContext dbContext,
        LSCoreContextUser currentUser,
        ILogManager logManager,
        ISettingManager settingManager,
        ITDKomercijalnoApiManager komercijalnoApiManager
    ) : base(logger, dbContext, currentUser)
    {
        _logger = logger;
        _dbContext = dbContext;
        _currentUser = currentUser;
        _logManager = logManager;
        _settingManager = settingManager;
        _komercijalnoApiManager = komercijalnoApiManager; 
        _httpClient.BaseAddress = new Uri(
            Constants.KomercijalnoApiUrlFormat
        );
    }
    public PartnerYearsDto GetPartnersReportByYearsKomercijalnoFinansijsko()
    {
        var response = new PartnerYearsDto();
        var defaultYearBehind =
            _settingManager.GetValueByKey(SettingKey.PARTNERI_PO_GODINAMA_KOMERCIJALNO_FINANSIJSKO_PERIOD_GODINA);

        response.Years =  Enumerable.Range(0, Convert.ToInt32(defaultYearBehind))
            .Select(i => new PartnerYearDto
            {
                Key = $"{DateTime.Now.Year - i}",
                Value = string.Format(Constants.PartnerIzvestajFinansijskoKomercijalnoLabelFormat, DateTime.Now.Year - i)
            })
            .ToList();

        response.DefaultTolerancija = Convert.ToInt32(_settingManager
            .GetValueByKey(SettingKey.PARTNERI_PO_GODINAMA_DEFAULT_TOLERANCIJA));

        return response;
    }

    public async Task<LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>> GetPartnersReportByYearsKomercijalnoFinansijskoDataAsync(GetPartnersReportByYearsKomercijalnoFinansijskoRequest request)
    {
            /* payload: [
            {
                ppid: 111,
                naziv: "Something",
                komercijalno: [
                    {
                        year: 2024,
                        pocetak: 100000,
                        kraj: 200000
                    },
                    ...
                ],
                finansijsko: [
                    {
                        year: 2024,
                        pocetak: 100000,
                        kraj: 200000
                    },
                    ...
                ]
            },
        ...
        ] */
        //get latest year from request and fetch partners
        int maxYear = request.Years.Max();

        /*_httpClient.BaseAddress = new Uri(
            String.Format(Constants.KomercijalnoApiUrlFormat, maxYear)
        );*/
        // uncomment this for prod
        var partnersResponse = await _httpClient.GetAsync(
            $"/partneri?pageSize={request.PageSize}&currentPage={request.CurrentPage}&sortDirection=1"
        );
        partnersResponse.HandleStatusCode();
        var partnersData = await partnersResponse.Content.ReadFromJsonAsync<LSCoreSortedAndPagedResponse<PartnerApiDto>>();

        
        var ppids = partnersData!.Payload!
            .Select(partner => partner.Ppid)
            .ToList();

        var payload = new List<GetPartnersReportByYearsKomercijalnoFinansijskoDto>();
        var komercijalnoKraj = new Dictionary<int, Dictionary<int, double>>();
        var komercijalnoPocetak = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoKraj = new Dictionary<int, Dictionary<int, double>>();
        var finansijkoPocetak = new Dictionary<int, Dictionary<int, double>>();


        foreach (int year in request.Years)
        {
            /*_httpClient.BaseAddress = new Uri(
                String.Format(Constants.KomercijalnoApiUrlFormat, year)
            );*/
            // uncomment this for prod

            var query = new List<string> { $"PPID={string.Join(",", ppids)}" };
            query.AddRange(Constants.DefaultPartnerIzvestajKomercijalnoDokumenti.Select(dok => $"VrDok={dok}"));

            var requestUri = $"/dokumenti?{string.Join("&", query)}";
            var dokumentiResponse = await _httpClient.GetAsync(requestUri);
            dokumentiResponse.HandleStatusCode();
            var dokumentiData = await dokumentiResponse.Content.ReadFromJsonAsync<List<DokumentiApiDto>>();

            requestUri = $"/istorija-uplata?{string.Join("&", ppids.Select(ppid => $"PPID={ppid}"))}";
            var istorijaUplataResponse = await _httpClient.GetAsync(requestUri);
            istorijaUplataResponse.HandleStatusCode();
            var istorijaUplataData = await istorijaUplataResponse.Content.ReadFromJsonAsync<List<IstorijaUplataApiDto>>();

            //preracunavanje komercijalnog poslovanja
            foreach (int ppid in ppids)
            {
                double psKupac = istorijaUplataData!.Where(x => x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -61 && x.PPID == ppid).Sum(x => x.Iznos);
                double psDobavljac = istorijaUplataData!.Where(x => x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -59 && x.PPID == ppid).Sum(x => x.Iznos);
                if (!komercijalnoPocetak.ContainsKey(year))
                    komercijalnoPocetak[year] = new Dictionary<int, double>();

                if (!komercijalnoPocetak[year].ContainsKey(ppid))
                    komercijalnoPocetak[year][ppid] = 0;

                komercijalnoPocetak[year][ppid] = psKupac - psDobavljac;
            }
            

            foreach (int ppid in ppids)
            {
                // 15, 14 = potrazuje = izlaz
                // 22 = potrazuje = ulaz
                // 39, 10 = duguje = ulaz-
                // 13, 40 = duguje = izlaz
                if (!komercijalnoKraj.ContainsKey(year))
                    komercijalnoKraj[year] = new Dictionary<int, double>();

                if (!komercijalnoKraj[year].ContainsKey(ppid))
                    komercijalnoKraj[year][ppid] = 0;

                komercijalnoKraj[year][ppid] -= (double)dokumentiData!.Where(x => new int[] { 13, 40 }.Contains(x.VrDok)).Sum(x => x.Duguje);
                komercijalnoKraj[year][ppid] -= (double)dokumentiData!.Where(x => new int[] { 14, 15 }.Contains(x.VrDok)).Sum(x => x.Potrazuje);
                komercijalnoKraj[year][ppid] += (double)dokumentiData!.Where(x => new int[] { 13, 14, 15, 40 }.Contains(x.VrDok) && new NacinUplate[] { NacinUplate.Gotovina, NacinUplate.Kartica }.Contains((NacinUplate)x.NuId)).Sum(x => x.Potrazuje);
                komercijalnoKraj[year][ppid] += (double)dokumentiData!.Where(x => new int[] { 10, 39 }.Contains(x.VrDok)).Sum(x => x.Duguje);
                komercijalnoKraj[year][ppid] += (double)dokumentiData!.Where(x => new int[] { 22 }.Contains(x.VrDok)).Sum(x => x.Potrazuje);
                komercijalnoKraj[year][ppid] -= (double)dokumentiData!.Where(x => new int[] { 10 }.Contains(x.VrDok) && x.NuId == NacinUplate.Gotovina).Sum(x => x.Duguje);
            }
        }
        foreach (int ppid in ppids)
        {
            var KomercijalnoDto = new List<YearStartEndDto>();
            foreach (int year in request.Years)
            {
                var singleDto = new YearStartEndDto();
                singleDto.Year = year;
                singleDto.Pocetak = komercijalnoPocetak.TryGetValue(year, out var startData) && startData.TryGetValue(ppid, out var startValue) ? 
                    (double) startValue
                    : 0;
                singleDto.Kraj = komercijalnoKraj.TryGetValue(year, out var endData) && startData.TryGetValue(ppid, out var endValue) ?
                    (double) endValue
                    : 0;
                KomercijalnoDto.Add(singleDto);
            }

            payload.Add(new GetPartnersReportByYearsKomercijalnoFinansijskoDto()
            {
                PPID = ppid,
                Naziv = "Test",
                Komercijalno = KomercijalnoDto,
                Finansijsko = new List<YearStartEndDto>()
            });
        }

        return new LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>()
        {
            Payload = payload,
            Pagination = new LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>.PaginationData(
                request.CurrentPage,
                request.PageSize,
                partnersData.Pagination.TotalCount
            )
        };
    }

    public async Task<List<PartnerDto>> GetRecentlyCreatedPartnersAsync()
    {
        var recentPartnersCreationLogs = Queryable<LogEntity>()
            .Where(x =>
                x.IsActive
                && x.Key == LogKey.NoviKomercijalnoPartner
                && x.CreatedAt >= DateTime.Now.AddDays(-7)
            )
            .OrderByDescending(x => x.CreatedAt);

        if (!recentPartnersCreationLogs.Any())
            return [];

        var partnerIds = recentPartnersCreationLogs.Select(x => Convert.ToInt32(x.Value)).ToArray();
        var resp = await _komercijalnoApiManager.GetPartnersAsync(
            new PartneriGetMultipleRequest { Ppid = partnerIds }
        );

        if (resp.Payload == null || resp.Payload.Count == 0)
            return [];

        return resp.Payload!.OrderBy(x => Array.IndexOf(partnerIds, x.Ppid)).ToList();
    }
}
