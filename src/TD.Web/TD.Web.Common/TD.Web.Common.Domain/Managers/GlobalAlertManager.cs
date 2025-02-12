using TD.Web.Common.Contracts.Requests.GlobalAlerts;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Domain.Managers;

public class GlobalAlertManager (IGlobalAlertRepository repository)
    : IGlobalAlertManager
{
    public List<GlobalAlertDto> GetMultiple(GlobalAlertsGetMultipleRequest request) =>
        repository.GetMultiple()
            .Where(x => x.Application == request.Application)
            .ToDtoList<GlobalAlertEntity, GlobalAlertDto>();
}