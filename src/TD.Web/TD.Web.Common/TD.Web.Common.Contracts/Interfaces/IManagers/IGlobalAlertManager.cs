using LSCore.Contracts.Http;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Requests.GlobalAlerts;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IGlobalAlertManager
    {
        LSCoreListResponse<GlobalAlertDto> GetMultiple(GlobalAlertsGetMultipleRequest request);
    }
}
