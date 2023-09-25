using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.ProductPrices;
using TD.Web.Contracts.Interfaces.IManagers;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class ProductsPricesController : ControllerBase
    {
        private readonly IProductPriceManager _productPriceManager;
        public ProductsPricesController(IProductPriceManager productPriceManager) 
        {
            _productPriceManager = productPriceManager;
        }
        [HttpGet]
        [Route("/products-prices")]
        public ListResponse<ProductsPricesGetDto> Get()
        {
            return _productPriceManager.GetMultiple();
        }
        [HttpDelete]
        [Route("/product-prices/{id}")]
        public Response<bool> Delete([FromRoute]IdRequest request)
        {
            return _productPriceManager.Delete(request);
        }
    }
}
