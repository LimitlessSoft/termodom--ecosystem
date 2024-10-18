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

namespace TD.Office.Public.Domain.Managers;

public class PartnerManager(
    ILogger<PartnerManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser,
    ILogManager logManager,
    ISettingManager settingManager,
    ITDKomercijalnoApiManager komercijalnoApiManager
) : LSCoreManagerBase<PartnerManager>(logger, dbContext, currentUser), IPartnerManager
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
