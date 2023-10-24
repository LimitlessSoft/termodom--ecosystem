using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.ProductsGroups;
using TD.Web.Contracts.Interfaces.IManagers;

namespace TD.Web.Api.Controllers
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
        public Response<ProductsGroupsGetDto> GetMultiple([FromRoute]int id)
        {
            return _productGroupManager.Get(new IdRequest(id));
        }
    }
}
