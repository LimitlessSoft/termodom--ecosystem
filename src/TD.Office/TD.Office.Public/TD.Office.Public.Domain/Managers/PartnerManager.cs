using System.Collections.Concurrent;
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
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Enums;
using TD.Komercijalno.Contracts.Requests.IstorijaUplata;
using TD.Office.Public.Contracts.Interfaces.Factories;
using TD.Komercijalno.Contracts.Requests.Izvodi;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using LSCore.Domain.Extensions;
using LSCore.Contracts.Dtos;
using LSCore.Contracts.Exceptions;

namespace TD.Office.Public.Domain.Managers;

public class PartnerManager(
    ILogger<PartnerManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser,
    ILogManager logManager,
    ISettingManager settingManager,
    IKomercijalnoIFinansijskoPoGodinamaStatusRepository komercijalnoIFinansijskoPoGodinamaStatusRepository,
    IKomercijalnoIFinansijskoPoGodinamaRepository komercijalnoIFinansijskoPoGodinamaRepository,
    ITDKomercijalnoApiManager komercijalnoApiManager,
    ITDKomercijalnoApiManagerFactory komercijalnoApiManagerFactory)
    : LSCoreManagerBase<PartnerManager, KomercijalnoIFinansijskoPoGodinamaEntity>(logger, dbContext, currentUser), IPartnerManager
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

        response.Status = komercijalnoIFinansijskoPoGodinamaStatusRepository.GetAllStatuses()
            .ToDtoList<KomercijalnoIFinansijskoPoGodinamaStatusEntity,LSCoreIdNamePairDto>();

        return response;
    }

    public async Task<LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>> GetPartnersReportByYearsKomercijalnoFinansijskoDataAsync(GetPartnersReportByYearsKomercijalnoFinansijskoRequest request)
    {
        Array.Sort(request.Years);
        var finalData = new ConcurrentBag<GetPartnersReportByYearsKomercijalnoFinansijskoDto>();

        var partnersCount = komercijalnoApiManager.GetPartnersCountAsync();
        
        var partners = await komercijalnoApiManager.GetPartnersAsync(
            new PartneriGetMultipleRequest()
            {
                PageSize = request.PageSize,
                CurrentPage = request.CurrentPage,
                SortDirection = request.SortDirection,
                SearchKeyword = request.SearchKeyword
            }
        );
        
        var ppids = partners.Payload.Select(x => x.Ppid).ToArray();
        
        var komercijalnoKraj = new Dictionary<int, Dictionary<int, double>>();
        var komercijalnoPocetak = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoKupacKraj = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoKupacPocetak = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoDobavljacKraj = new Dictionary<int, Dictionary<int, double>>();
        var finansijskoDobavljacPocetak = new Dictionary<int, Dictionary<int, double>>();

        foreach (var year in request.Years)
        {
            var yearApi = komercijalnoApiManagerFactory.Create(year);

            var dokumentiDataTask = yearApi.GetMultipleDokumentAsync(
                new Komercijalno.Contracts.Requests.Dokument.DokumentGetMultipleRequest()
                {
                    VrDok = [ 10, 13, 14, 15, 22, 39, 40 ],
                    PPID = ppids
                }
            );

            var istorijaUplataDataTask = yearApi.GetMultipleIstorijaUplataAsync(
                new IstorijaUplataGetMultipleRequest()
                {
                    PPID = ppids
                }
            );

            var promeneDobavljacDataTask = yearApi.GetMultiplePromeneAsync(
                new Komercijalno.Contracts.Requests.Promene.PromenaGetMultipleRequest()
                {
                    PPID = ppids,
                    KontoStartsWith = "43"
                }
            );

            var promeneKupacDataTask = yearApi.GetMultiplePromeneAsync(
                new Komercijalno.Contracts.Requests.Promene.PromenaGetMultipleRequest()
                {
                    PPID = ppids,
                    KontoStartsWith = "204"
                }
            );

            var izvodiDataTask = yearApi.GetMultipleIzvodAsync(
                new IzvodGetMultipleRequest()
                {
                    PPID = ppids
                }
            );

            var istorijaUplataData = await istorijaUplataDataTask;
            var promeneDobavljacData = await promeneDobavljacDataTask;
            var promeneKupacData = await promeneKupacDataTask;
            var izvodiData = await izvodiDataTask;
            var dokumentiData = await dokumentiDataTask;

            var mappedIstorijaUplataByPPID =
                istorijaUplataData.GroupBy(x => x.PPID).ToDictionary(x => x.Key, x => x.ToList());
            var mappedPromeneDobavljacByPPID =
                promeneDobavljacData.GroupBy(x => x.PPID).ToDictionary(x => x.Key, x => x.ToList());
            var mappedPromeneKupacByPPID =
                promeneKupacData.GroupBy(x => x.PPID).ToDictionary(x => x.Key, x => x.ToList());
            var mappedIzvodiByPPID = izvodiData.GroupBy(x => x.PPID).ToDictionary(x => x.Key, x => x.ToList());
            var mappedDokumentiByPPID = dokumentiData.GroupBy(x => x.PPID).ToDictionary(x => x.Key, x => x.ToList());

            // preracunavanje komercijalnog poslovanja
            foreach (var ppid in ppids)
            {
                var psKupac = !mappedIstorijaUplataByPPID.ContainsKey(ppid) ? 0 : mappedIstorijaUplataByPPID[ppid]
                    .Where(x =>
                        x.Datum.Day == 1
                        && x.Datum.Month == 1
                        && x.VrDok == -61)
                    .Sum(x => x.Iznos);

                var psDobavljac = !mappedIstorijaUplataByPPID.ContainsKey(ppid) ? 0 : mappedIstorijaUplataByPPID[ppid]
                    .Where(x =>
                        x.Datum.Day == 1
                        && x.Datum.Month == 1
                        && x.VrDok == -59)
                    .Sum(x => x.Iznos);

                var pocetnoFinansijskoKupac = !mappedPromeneKupacByPPID.ContainsKey(ppid) ? 0 : mappedPromeneKupacByPPID[ppid]
                    .Where(x =>
                        (x.VrDok == -61 || x.VrDok == 0))
                    .Sum(x => x.Potrazuje - x.Duguje) ?? 0;

                var pocetnoFinansijskoDobavljac = !mappedPromeneDobavljacByPPID.ContainsKey(ppid) ? 0 : mappedPromeneDobavljacByPPID[ppid]
                    .Where(x =>
                        (x.VrDok == -59 || x.VrDok == 0))
                    .Sum(x => x.Potrazuje - x.Duguje) ?? 0;

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

                foreach (var dd in mappedDokumentiByPPID.TryGetValue(ppid, out var ddList) ? ddList : [])
                {
                    // komercijalnoKraj[year][ppid] -= (double)(await dokumentiData)!.Where(x => new [] { 13, 40 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Duguje);
                    // komercijalnoKraj[year][ppid] -= (double)(await dokumentiData)!.Where(x => new [] { 14, 15 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Potrazuje);
                    // komercijalnoKraj[year][ppid] += (double)(await dokumentiData)!.Where(x => new [] { 13, 14, 15, 40 }.Contains(x.VrDok) && x.PPID == ppid && new NacinUplate[] { NacinUplate.Gotovina, NacinUplate.Kartica }.Contains((NacinUplate)x.NuId)).Sum(x => x.Potrazuje);
                    // komercijalnoKraj[year][ppid] += (double)(await dokumentiData)!.Where(x => new [] { 10, 39 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Duguje);
                    // komercijalnoKraj[year][ppid] += (double)(await dokumentiData)!.Where(x => new [] { 22 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.Potrazuje);
                    // komercijalnoKraj[year][ppid] -= (double)(await dokumentiData)!.Where(x => new [] { 10 }.Contains(x.VrDok) && x.PPID == ppid && x.NuId == (short)NacinUplate.Gotovina).Sum(x => x.Duguje);
                    if ((dd.VrDok == 13 || dd.VrDok == 40))
                        komercijalnoKraj[year][ppid] -= (double)dd.Duguje;

                    if ((dd.VrDok == 14 || dd.VrDok == 15))
                        komercijalnoKraj[year][ppid] -= (double)dd.Potrazuje;

                    if ((dd.VrDok == 13 || dd.VrDok == 14 || dd.VrDok == 15 || dd.VrDok == 40) &&
                        new [] { NacinUplate.Gotovina, NacinUplate.Kartica }.Contains((NacinUplate)dd.NuId))
                        komercijalnoKraj[year][ppid] += (double)dd.Potrazuje;

                    if ((dd.VrDok == 10 || dd.VrDok == 39))
                        komercijalnoKraj[year][ppid] += (double)dd.Duguje;

                    if (dd.VrDok == 22 && dd.NuId != (short)NacinUplate.Kartica && dd.NuId != (short)NacinUplate.Gotovina)
                        komercijalnoKraj[year][ppid] += (double)dd.Potrazuje;

                    if (dd.VrDok == 10 && dd.NuId == (short)NacinUplate.Gotovina)
                        komercijalnoKraj[year][ppid] -= (double)dd.Duguje;
                }

                //komercijalnoKraj[year][ppid] += (double)istorijaUplataData!.Where(x => new [] { 91 }.Contains(x.VrDok) && x.PPID == ppid).Sum(x => x.IO == 0 ? x.Iznos : x.Iznos * -1);

                foreach (var id in mappedIzvodiByPPID.TryGetValue(ppid, out var idList) ? idList : [])
                {
                    // komercijalnoKraj[year][ppid] -= (double)(await izvodiData)!.Where(x => x.PPID == ppid).Sum(x => x.Duguje);
                    // komercijalnoKraj[year][ppid] += (double)(await izvodiData)!.Where(x => x.PPID == ppid).Sum(x => x.Potrazuje);

                    komercijalnoKraj[year][ppid] -= (double)id.Duguje;
                    komercijalnoKraj[year][ppid] += (double)id.Potrazuje;
                }

                if (!finansijskoDobavljacKraj.ContainsKey(year))
                    finansijskoDobavljacKraj[year] = new Dictionary<int, double>();

                if (!finansijskoKupacKraj.ContainsKey(year))
                    finansijskoKupacKraj[year] = new Dictionary<int, double>();

                foreach (var pk in mappedPromeneKupacByPPID.TryGetValue(ppid, out var pkList) ? pkList : [])
                {
                    // finansijskoKupacKraj[year][ppid] = (double)(await promeneKupacData)!.Where(x => x.PPID == ppid && x.VrDok != null).Sum(x => x.Potrazuje);
                    // finansijskoKupacKraj[year][ppid] -= (double)(await promeneKupacData)!.Where(x => x.PPID == ppid && x.VrDok != null).Sum(x => x.Duguje);

                    if (!finansijskoKupacKraj[year].ContainsKey(ppid))
                        finansijskoKupacKraj[year][ppid] = 0;

                    finansijskoKupacKraj[year][ppid] += (double)pk.Potrazuje;

                    if (!finansijskoKupacKraj[year].ContainsKey(ppid))
                        finansijskoKupacKraj[year][ppid] = 0;
                    finansijskoKupacKraj[year][ppid] -= (double)pk.Duguje;
                }

                foreach (var pd in mappedPromeneDobavljacByPPID.TryGetValue(ppid, out var pdList) ? pdList : [])
                {
                    // finansijskoDobavljacKraj[year][ppid] = (double)(await promeneDobavljacData)!.Where(x => x.PPID == ppid).Sum(x => x.Potrazuje);
                    // finansijskoDobavljacKraj[year][ppid] -= (double)(await promeneDobavljacData)!.Where(x => x.PPID == ppid).Sum(x => x.Duguje);

                    if (!finansijskoDobavljacKraj[year].ContainsKey(ppid))
                        finansijskoDobavljacKraj[year][ppid] = 0;

                    finansijskoDobavljacKraj[year][ppid] += (double)pd.Potrazuje;

                    if (!finansijskoDobavljacKraj[year].ContainsKey(ppid))
                        finansijskoDobavljacKraj[year][ppid] = 0;
                    finansijskoDobavljacKraj[year][ppid] -= (double)pd.Duguje;
                }
            }
        }
        var statusDefaultId = komercijalnoIFinansijskoPoGodinamaStatusRepository.GetDefaultId();
        //format response
        foreach (var ppid in ppids)
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

            var isOk = true;
            for (var i = 0; i < KomercijalnoDto.Count; i++)
            {
                if (Math.Abs(KomercijalnoDto[i].Pocetak -
                             (FinansijskoKupacDto[i].Pocetak + FinansijskoDobavljacDto[i].Pocetak)) >
                    request.Tolerancija
                    || ((KomercijalnoDto[i].Year != DateTime.Now.Year) && Math.Abs(KomercijalnoDto[i].Kraj -
                                (FinansijskoKupacDto[i].Kraj + FinansijskoDobavljacDto[i].Kraj)) > request.Tolerancija))
                {
                    isOk = false;
                    break;
                }

                if (i == 0) // doing like this to avoid 1 selected year
                    continue;
                
                if(Math.Abs(KomercijalnoDto[i].Pocetak - KomercijalnoDto[i - 1].Kraj) > request.Tolerancija
                   || Math.Abs(FinansijskoKupacDto[i].Pocetak - FinansijskoKupacDto[i - 1].Kraj) > request.Tolerancija
                   || Math.Abs(FinansijskoDobavljacDto[i].Pocetak - FinansijskoDobavljacDto[i - 1].Kraj) > request.Tolerancija)
                {
                    isOk = false;
                    break;
                }
            }
            
            if (isOk)
                continue;

            KomercijalnoIFinansijskoPoGodinamaEntity entity;
            try
            {
                entity = komercijalnoIFinansijskoPoGodinamaRepository.GetByPPID(ppid);
            }
            catch(LSCoreNotFoundException e)
            {
                entity = Insert(new KomercijalnoIFinansijskoPoGodinamaEntity()
                {
                    PPID = ppid,
                    StatusId = statusDefaultId,
                });
            }
            

            finalData.Add(new GetPartnersReportByYearsKomercijalnoFinansijskoDto()
            {
                Status = entity.StatusId,
                Komentar = entity.Comment,
                PPID = entity.PPID,
                Naziv = partners.Payload.FirstOrDefault(x => x.Ppid == ppid)?.Naziv ?? "Nema naziv",
                Komercijalno = KomercijalnoDto,
                FinansijskoKupac = FinansijskoKupacDto,
                FinansijskoDobavljac = FinansijskoDobavljacDto
            });
        }
        
        return new LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>()
        {
            Payload = finalData.ToList(),
            Pagination = new LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>.PaginationData(
                request.CurrentPage,
                request.PageSize,
                await partnersCount
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

    public bool SaveKomercijalnoFinansijskoKomentar(SaveKomercijalnoFinansijskoCommentRequest request)
    {
        request.Validate();
        var entity = komercijalnoIFinansijskoPoGodinamaRepository.GetByPPID(request.PPID);
        entity.Comment = request.Komentar;
        Update(entity);

        return true;
    }

    public bool SaveKomercijalnoFinansijskoStatus(SaveKomercijalnoFinansijskoStatusRequest request)
    {
        request.Validate();
        var entity = komercijalnoIFinansijskoPoGodinamaRepository.GetByPPID(request.PPID);
        entity.StatusId = request.StatusId;
        Update(entity);

        return true;
    }
}
