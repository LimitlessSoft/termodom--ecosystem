using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class ProductsGroupsController : ControllerBase
    {
        private readonly IProductGroupManager _productGroupManager;

        public ProductsGroupsController(IProductGroupManager productGroupManager, IHttpContextAccessor httpContextAccessor)
        {
            _productGroupManager = productGroupManager;
            _productGroupManager.SetContextInfo(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("/products-groups")]
        public ListResponse<ProductsGroupsGetDto> GetMultiple()
        {
            return _productGroupManager.GetMultiple();
        }

        [HttpGet]
        [Route("/products-groups/{id}")]
        public Response<ProductsGroupsGetDto> Get([FromRoute]int id)
        {
            return _productGroupManager.Get(new IdRequest(id));
        }

        [HttpPut]
        [Route("/products-groups")]
        public Response<long> Save([FromBody]ProductsGroupsSaveRequest request)
        {
            return _productGroupManager.Save(request);
        }
    }
}
