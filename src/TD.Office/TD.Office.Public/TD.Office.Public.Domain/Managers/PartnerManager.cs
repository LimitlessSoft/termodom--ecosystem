using LSCore.Contracts;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Responses;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.IManagers;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Partners;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Domain.Managers;

public class PartnerManager(
    ILogger<PartnerManager> logger,
    OfficeDbContext dbContext,
    LSCoreContextUser currentUser,
    ILogManager logManager,
    ITDKomercijalnoApiManager komercijalnoApiManager
) : LSCoreManagerBase<PartnerManager>(logger, dbContext, currentUser), IPartnerManager
{
    public async Task<List<PartnerDto>> GetRecentlyCreatedPartnersAsync()
    {
        var recentPartnersCreationLogs = Queryable<LogEntity>()
            .Where(x =>
                x.IsActive
                && x.Key == LogKey.NoviKomercijalnoPartner
                && x.CreatedAt >= DateTime.Now.AddDays(-7)
            )
            .OrderByDescending(x => x.CreatedAt);

        var partnerIds = recentPartnersCreationLogs.Select(x => Convert.ToInt32(x.Value)).ToArray();
        var resp = await komercijalnoApiManager.GetPartnersAsync(
            new PartneriGetMultipleRequest { Ppid = partnerIds }
        );

        if (resp.Payload == null || resp.Payload.Count == 0)
            return [];

        return resp.Payload!.OrderBy(x => Array.IndexOf(partnerIds, x.Ppid)).ToList();
    }
}
