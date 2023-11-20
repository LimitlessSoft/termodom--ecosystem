using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.ProductsGroups;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class ProductsGroupsController : ControllerBase
    {
        private readonly IProductGroupManager _productGroupManager;

        public ProductsGroupsController(IProductGroupManager productGroupManager)
        {
            _productGroupManager = productGroupManager;
        }

        [HttpGet]
        [Route("/products-groups")]
        public LSCoreListResponse<LSCoreIdNamePairDto> Index([FromQuery] ProductsGroupsGetRequest request) =>
            _productGroupManager.GetMultiple(request);
    }
}
