using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class ProductsGroupsController : ControllerBase
    {
        private readonly IProductGroupManager _productGroupManager;

        public ProductsGroupsController(IProductGroupManager productGroupManager, IHttpContextAccessor httpContextAccessor)
        {
            _productGroupManager = productGroupManager;
            _productGroupManager.SetContext(httpContextAccessor.HttpContext!);
        }

        [HttpGet]
        [Route("/products-groups")]
        public LSCoreListResponse<ProductsGroupsGetDto> GetMultiple() =>
            _productGroupManager.GetMultiple();

        [HttpGet]
        [Route("/products-groups/{id}")]
        public LSCoreResponse<ProductsGroupsGetDto> Get([FromRoute]int id) =>
            _productGroupManager.Get(new LSCoreIdRequest() { Id = id });

        [HttpPut]
        [Route("/products-groups")]
        public LSCoreResponse<long> Save([FromBody]ProductsGroupsSaveRequest request) =>
            _productGroupManager.Save(request);

        [HttpDelete]
        [Route("/products-groups/{Id}")]
        public LSCoreResponse Delete([FromRoute]ProductsGroupsDeleteRequest request) =>
            _productGroupManager.Delete(request);
    }
}
