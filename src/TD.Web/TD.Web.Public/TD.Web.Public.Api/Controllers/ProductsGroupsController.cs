using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.ProductsGroups;

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
        [Route("/products-groups/{name}")]
        public LSCoreResponse<ProductsGroupsGetDto> Get([FromRoute] string name) =>
            _productGroupManager.Get(name);

        [HttpGet]
        [Route("/products-groups")]
        public LSCoreListResponse<ProductsGroupsGetDto> GetMultiple([FromQuery] ProductsGroupsGetRequest request) =>
            _productGroupManager.GetMultiple(request);
    }
}
