using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class EnumsController : ControllerBase
    {
        private readonly IEnumManager _enumManager;
        public EnumsController(IEnumManager enumManager)
        {
            _enumManager = enumManager;
        }

        [HttpGet]
        [Route("/order-statuses")]
        public LSCoreListResponse<LSCoreIdNamePairDto> GetOrderStatuses() =>
            _enumManager.GetOrderStatuses();
        
        [HttpGet]
        [Route("/user-types")]
        public LSCoreListResponse<LSCoreIdNamePairDto> GetUserTypes() =>
            _enumManager.GetUserTypes();
        
        [HttpGet]
        [Route("/product-group-types")]
        public LSCoreListResponse<LSCoreIdNamePairDto> GetProductGroupTypes() =>
            _enumManager.GetProductGroupTypes();
    }
}
