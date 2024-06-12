using TD.Web.Common.Contracts.Requests.GlobalAlerts;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class GlobalAlertsController (IGlobalAlertManager globalAlertManager) : ControllerBase
{
    [HttpGet]
    [Route("/global-alerts")]
    public List<GlobalAlertDto> GetMultiple([FromQuery] GlobalAlertsGetMultipleRequest request) =>
        globalAlertManager.GetMultiple(request);
}