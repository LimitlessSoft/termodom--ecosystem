using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Interfaces.Managers;
using TD.Web.Admin.Contracts.Requests.Products;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;

        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        [Route("/products/{id}")]
        public LSCoreResponse<ProductsGetDto> Get([FromRoute] int id)
        {
            return _productManager.Get(new LSCoreIdRequest() { Id = id });
        }

        [HttpGet]
        [Route("/products")]
        public LSCoreListResponse<ProductsGetDto> GetMultiple([FromQuery] ProductsGetMultipleRequest request)
        {
            return _productManager.GetMultiple(request);
        }

        [HttpPut]
        [Route("/products")]
        public LSCoreResponse<long> Save([FromBody] ProductsSaveRequest request)
        {
            return _productManager.Save(request);
        }

        [HttpGet]
        [Route("/products-search")]
        public LSCoreListResponse<ProductsGetDto> GetSearch([FromQuery] ProductsGetSearchRequest request)
        {
            return _productManager.GetSearch(request);
        }

        [HttpGet]
        [Route("/products-classifications")]
        public LSCoreListResponse<LSCoreIdNamePairDto> GetClassifications()
        {
            return _productManager.GetClassifications();
        }
    }
}
