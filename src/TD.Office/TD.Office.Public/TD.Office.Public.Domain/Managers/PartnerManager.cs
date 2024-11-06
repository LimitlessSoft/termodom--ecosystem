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
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.IstorijaUplata;

namespace TD.Office.Public.Domain.Managers;

public class PartnerManager(
    ILogger<PartnerManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser,
    ILogManager logManager,
    ISettingManager settingManager,
    ITDKomercijalnoApiManager komercijalnoApiManager)
    : LSCoreManagerBase<PartnerManager>(logger, dbContext, currentUser), IPartnerManager
{
    public PartnerYearsDto GetPartnersReportByYearsKomercijalnoFinansijsko()
    {
        var response = new PartnerYearsDto();
        var defaultYearBehind =
            settingManager.GetValueByKey(SettingKey.PARTNERI_PO_GODINAMA_KOMERCIJALNO_FINANSIJSKO_PERIOD_GODINA);

        response.Years =  Enumerable.Range(0, Convert.ToInt32(defaultYearBehind))
            .Select(i => new PartnerYearDto
            {
                Key = $"{DateTime.Now.Year - i}",
                Value = string.Format(Constants.PartnerIzvestajFinansijskoKomercijalnoLabelFormat, DateTime.Now.Year - i)
            })
            .ToList();

        response.DefaultTolerancija = Convert.ToInt32(settingManager
            .GetValueByKey(SettingKey.PARTNERI_PO_GODINAMA_DEFAULT_TOLERANCIJA));

        return response;
    }

    public async Task<LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>> GetPartnersReportByYearsKomercijalnoFinansijskoDataAsync(GetPartnersReportByYearsKomercijalnoFinansijskoRequest request)
    {
        var partnersData = await komercijalnoApiManager.GetPartnersAsync(
            new PartneriGetMultipleRequest()
            {
                PageSize = request.PageSize,
                CurrentPage = request.CurrentPage,
                SortDirection = 0
            }
        );
        
        var ppids = partnersData!.Payload!
            .Select(partner => partner.Ppid)
            .ToArray();

        var payload = new List<GetPartnersReportByYearsKomercijalnoFinansijskoDto>();
        var komercijalnoKraj = new Dictionary<int, Dictionary<int, double>>();
        var komercijalnoPocetak = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoKupacKraj = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoKupacPocetak = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoDobavljacKraj = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoDobavljacPocetak = new Dictionary<int, Dictionary<int, double>>();

        foreach (var year in request.Years)
        {
            var dokumentiData = await komercijalnoApiManager.GetMultipleDokumentAsync(
                new Komercijalno.Contracts.Requests.Dokument.DokumentGetMultipleRequest()
                {
                    PPID = ppids
                }
            );

            var istorijaUplataData = await komercijalnoApiManager.GetMultipleIstorijaUplataAsync(
                new IstorijaUplataGetMultipleRequest()
                {
                    PPID = ppids
                }
            );

            var promeneDobavljacData = await komercijalnoApiManager.GetMultiplePromeneAsync(
                new Komercijalno.Contracts.Requests.Promene.PromenaGetMultipleRequest()
                {
                    PPID = ppids,
                    KontoStartsWith = "43"
                }
            );

            var promeneKupacData = await komercijalnoApiManager.GetMultiplePromeneAsync(
                new Komercijalno.Contracts.Requests.Promene.PromenaGetMultipleRequest()
                {
                    PPID = ppids,
                    KontoStartsWith = "204"
                }
            );

            // preracunavanje komercijalnog poslovanja
            foreach (var ppid in ppids)
            {
                var psKupac = istorijaUplataData!.Where(x => x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -61 && x.PPID == ppid).Sum(x => x.Iznos);
                var psDobavljac = istorijaUplataData!.Where(x => x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -59 && x.PPID == ppid).Sum(x => x.Iznos);
                var pocetnoFinansijskoKupac = (double)promeneKupacData!.Where(x => x.PPID == ppid && (x.VrDok == -61 || x.VrDok == 0)).Sum(x => x.Potrazuje - x.Duguje);
                var pocetnoFinansijskoDobavljac = (double)promeneDobavljacData!.Where(x => x.PPID == ppid && (x.VrDok == -59 || x.VrDok == 0)).Sum(x => x.Potrazuje - x.Duguje);
                
                if (!komercijalnoPocetak.ContainsKey(year))
                    komercijalnoPocetak[year] = new Dictionary<int, double>();
                if (!finansijskoDobavljacPocetak.ContainsKey(year))
                    finansijskoDobavljacPocetak[year] = new Dictionary<int, double>();
                if (!finansijskoKupacPocetak.ContainsKey(year))
                    finansijskoKupacPocetak[year] = new Dictionary<int, double>();

                komercijalnoPocetak[year][ppid] = psKupac - psDobavljac;
                finansijskoKupacPocetak[year][ppid] = pocetnoFinansijskoKupac;
                finansijskoDobavljacPocetak[year][ppid] = pocetnoFinansijskoDobavljac;

                // 15, 14 = potrazuje = izlaz
                // 22 = potrazuje = ulaz
                // 39, 10 = duguje = ulaz
                // 13, 40 = duguje = izlaz

                if (!komercijalnoKraj.ContainsKey(year))
                    komercijalnoKraj[year] = new Dictionary<int, double>();
                komercijalnoKraj[year][ppid] = komercijalnoPocetak[year][ppid];

                komercijalnoKraj[year][ppid] -= (double)dokumentiData!.Where(x => new int[] { 13, 40 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Duguje);
                komercijalnoKraj[year][ppid] -= (double)dokumentiData!.Where(x => new int[] { 14, 15 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Potrazuje);
                komercijalnoKraj[year][ppid] += (double)dokumentiData!.Where(x => new int[] { 13, 14, 15, 40 }.Contains(x.VrDok) && x.PPID == ppid && new NacinUplate[] { NacinUplate.Gotovina, NacinUplate.Kartica }.Contains((NacinUplate)x.NuId)).Sum(x => x.Potrazuje);
                komercijalnoKraj[year][ppid] += (double)dokumentiData!.Where(x => new int[] { 10, 39 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Duguje);
                komercijalnoKraj[year][ppid] += (double)dokumentiData!.Where(x => new int[] { 22 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Potrazuje);
                komercijalnoKraj[year][ppid] -= (double)dokumentiData!.Where(x => new int[] { 10 }.Contains(x.VrDok) && x.PPID == ppid && x.NuId == (short)NacinUplate.Gotovina).Sum(x => x.Duguje);


                if (!finansijskoDobavljacKraj.ContainsKey(year))
                    finansijskoDobavljacKraj[year] = new Dictionary<int, double>();

                if (!finansijskoKupacKraj.ContainsKey(year))
                    finansijskoKupacKraj[year] = new Dictionary<int, double>();


                finansijskoKupacKraj[year][ppid] = (double)promeneKupacData!.Where(x => x.PPID == ppid && x.VrDok != null).Sum(x => x.Potrazuje - x.Duguje);
                finansijskoDobavljacKraj[year][ppid] = (double)promeneDobavljacData!.Where(x => x.PPID == ppid).Sum(x => x.Potrazuje - x.Duguje);
            }
        }

        //format response
        foreach (int ppid in ppids)
        {
            var KomercijalnoDto = new List<YearStartEndDto>();
            var FinansijskoKupacDto = new List<YearStartEndDto>();
            var FinansijskoDobavljacDto = new List<YearStartEndDto>();
            foreach (var year in request.Years)
            {
                // Komercijalno mapping
                var komercijalnoDto = new YearStartEndDto
                {
                    Year = year,
                    Pocetak = komercijalnoPocetak.TryGetValue(year, out var startData) && startData.TryGetValue(ppid, out var startValue)
                        ? (double)startValue
                        : 0,
                    Kraj = komercijalnoKraj.TryGetValue(year, out var endData) && endData.TryGetValue(ppid, out var endValue)
                        ? (double)endValue
                        : 0
                };
                KomercijalnoDto.Add(komercijalnoDto);

                // FinansijskoKupac mapping
                var finansijskoKupacDto = new YearStartEndDto
                {
                    Year = year,
                    Pocetak = finansijskoKupacPocetak.TryGetValue(year, out var kupacStartData) && kupacStartData.TryGetValue(ppid, out var kupacStartValue)
                        ? (double)kupacStartValue
                        : 0,
                    Kraj = finansijskoKupacKraj.TryGetValue(year, out var kupacEndData) && kupacEndData.TryGetValue(ppid, out var kupacEndValue)
                        ? (double)kupacEndValue
                        : 0
                };
                FinansijskoKupacDto.Add(finansijskoKupacDto);

                // FinansijskoDobavljac mapping
                var finansijskoDobavljacDto = new YearStartEndDto
                {
                    Year = year,
                    Pocetak = finansijskoDobavljacPocetak.TryGetValue(year, out var dobavljacStartData) && dobavljacStartData.TryGetValue(ppid, out var dobavljacStartValue)
                        ? (double)dobavljacStartValue
                        : 0,
                    Kraj = finansijskoDobavljacKraj.TryGetValue(year, out var dobavljacEndData) && dobavljacEndData.TryGetValue(ppid, out var dobavljacEndValue)
                        ? (double)dobavljacEndValue
                        : 0
                };
                FinansijskoDobavljacDto.Add(finansijskoDobavljacDto);
            }

            payload.Add(new GetPartnersReportByYearsKomercijalnoFinansijskoDto()
            {
                PPID = ppid,
                Naziv = partnersData!.Payload.Where(x => x.Ppid == ppid).Select(x => x.Naziv).First() ?? "Nema naziv",
                Komercijalno = KomercijalnoDto,
                FinansijskoKupac = FinansijskoKupacDto,
                FinansijskoDobavljac = FinansijskoDobavljacDto
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
        var resp = await komercijalnoApiManager.GetPartnersAsync(
            new PartneriGetMultipleRequest { Ppid = partnerIds }
        );

        if (resp.Payload == null || resp.Payload.Count == 0)
            return [];

        return resp.Payload!.OrderBy(x => Array.IndexOf(partnerIds, x.Ppid)).ToList();
    }
}
