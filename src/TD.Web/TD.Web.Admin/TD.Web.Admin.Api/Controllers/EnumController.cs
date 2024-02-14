using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class EnumController : ControllerBase
    {
        private readonly IEnumManager _enumManager;
        public EnumController(IEnumManager enumManager)
        {
            _enumManager = enumManager;
        }

        [HttpGet]
        [Route("/order-statuses")]
        public LSCoreListResponse<LSCoreIdNamePairDto> GetOrderStatuses() =>
            _enumManager.GetOrderStatuses();
    }
}
