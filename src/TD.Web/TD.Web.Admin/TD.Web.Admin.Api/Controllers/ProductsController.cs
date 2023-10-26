using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
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
        public Response<ProductsGetDto> Get([FromRoute] int id)
        {
            return _productManager.Get(new IdRequest(id));
        }

        [HttpGet]
        [Route("/products")]
        public ListResponse<ProductsGetDto> GetMultiple([FromQuery] ProductsGetMultipleRequest request)
        {
            return _productManager.GetMultiple(request);
        }

        [HttpPut]
        [Route("/products")]
        public Response<long> Save([FromBody] ProductsSaveRequest request)
        {
            return _productManager.Save(request);
        }

        [HttpGet]
        [Route("/products-search")]
        public ListResponse<ProductsGetDto> GetSearch([FromQuery] ProductsGetSearchRequest request)
        {
            return _productManager.GetSearch(request);
        }

        [HttpGet]
        [Route("/products-classifications")]
        public ListResponse<IdNamePairDto> GetClassifications()
        {
            return _productManager.GetClassifications();
        }
    }
}
