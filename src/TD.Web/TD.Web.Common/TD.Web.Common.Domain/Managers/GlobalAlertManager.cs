using TD.Web.Common.Contracts.Requests.GlobalAlerts;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;

namespace TD.Web.Common.Domain.Managers;

public class GlobalAlertManager (ILogger<GlobalAlertManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<GlobalAlertManager, GlobalAlertEntity>(logger, dbContext), IGlobalAlertManager
{
    public List<GlobalAlertDto> GetMultiple(GlobalAlertsGetMultipleRequest request) =>
        Queryable()
            .Where(x => x.IsActive &&
                        x.Application == request.Application)
            .ToDtoList<GlobalAlertEntity, GlobalAlertDto>();
}