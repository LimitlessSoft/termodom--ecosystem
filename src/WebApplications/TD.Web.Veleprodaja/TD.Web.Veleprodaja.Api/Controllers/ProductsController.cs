using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Web.Veleprodaja.Contracts.Dtos.Products;
using TD.Web.Veleprodaja.Contracts.IManagers;
using TD.Web.Veleprodaja.Contracts.Requests.Products;

namespace TD.Web.Veleprodaja.Api.Controllers
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
        public ListResponse<ProductsGetDto> GetMultiple([FromQuery]ProductsGetRequest request)
        {
            return _productManager.GetMultiple(request);
        }

        [HttpPut]
        [Route("/products")]
        public Response<ProductsGetDto> Put([FromBody]ProductsPutRequest request)
        {
            return _productManager.Put(request);
        }
    }
}
