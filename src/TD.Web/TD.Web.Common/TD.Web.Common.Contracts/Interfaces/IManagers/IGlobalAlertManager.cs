using TD.Web.Common.Contracts.Requests.GlobalAlerts;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IGlobalAlertManager
{
    List<GlobalAlertDto> GetMultiple(GlobalAlertsGetMultipleRequest request);
}