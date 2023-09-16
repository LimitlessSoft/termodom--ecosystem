using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.Managers;
using TD.Web.Contracts.Requests.Products;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductManager _productManager;

        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        [Route("/products")]
        public ListResponse<ProductsGetMultipleDto> GetMultiple([FromQuery] ProductsGetMultipleRequest request)
        {
            return _productManager.GetMultiple(request);
        }

        [HttpGet]
        [Route("/products-search")]
        public ListResponse<ProductsGetMultipleDto> GetSearch([FromQuery] ProductsGetSearchRequest request)
        {
            return _productManager.GetSearch(request);
        }
    }
}
