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
        var partnersResponse = await _httpClient.GetAsync(
            $"/partneri?pageSize={request.PageSize}&currentPage={request.CurrentPage}"
        );
        partnersResponse.HandleStatusCode();
        // Fetch Komercijalno EP-s
        // Calculate
        // Format response
        return new LSCoreSortedAndPagedResponse<GetPartnersReportByYearsKomercijalnoFinansijskoDto>();
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
