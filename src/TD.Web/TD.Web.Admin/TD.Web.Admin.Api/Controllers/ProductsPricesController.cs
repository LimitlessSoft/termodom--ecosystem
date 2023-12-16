using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;

namespace TD.Web.Admin.Api.Controllers
{
    //[Authorize]
    [ApiController]
    public class ProductsPricesController : ControllerBase
    {
        private readonly IProductPriceManager _productPriceManager;

        public ProductsPricesController(IProductPriceManager productPriceManager, IHttpContextAccessor httpContextAccessor) 
        {
            _productPriceManager = productPriceManager;
            _productPriceManager.SetContext(httpContextAccessor.HttpContext!);
        }

        [HttpGet]
        // [Authorize("TestPolicy")]
        [Route("/products-prices")]
        public LSCoreListResponse<ProductsPricesGetDto> GetMultiple() =>
            _productPriceManager.GetMultiple();

        [HttpPut]
        [Route("/products-prices")]
        public LSCoreResponse<long> Save(SaveProductPriceRequest request) =>
            _productPriceManager.Save(request);

        [HttpDelete]
        [Route("/products-prices/{id}")]
        public LSCoreResponse Delete([FromRoute]LSCoreIdRequest request) =>
            _productPriceManager.Delete(request);
    }
}
