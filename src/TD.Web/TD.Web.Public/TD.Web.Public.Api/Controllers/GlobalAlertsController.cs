using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.GlobalAlerts;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class GlobalAlertsController : ControllerBase
    {
        private readonly IGlobalAlertManager _globalAlertManager;

        public GlobalAlertsController(IGlobalAlertManager globalAlertManager)
        {
            _globalAlertManager = globalAlertManager;
        }

        [HttpGet]
        [Route("/global-alerts")]
        public LSCoreListResponse<GlobalAlertDto> GetMultiple([FromQuery] GlobalAlertsGetMultipleRequest request) =>
            _globalAlertManager.GetMultiple(request);
    }
}
