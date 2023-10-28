using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
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
        private class CurrentUser
        {
            public int Id { get; set; }
        }

        public ProductsPricesController(IProductPriceManager productPriceManager, IHttpContextAccessor httpContextAccessor) 
        {
            _productPriceManager = productPriceManager;
            _productPriceManager.SetContextInfo(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        // [Authorize("TestPolicy")]
        [Route("/products-prices")]
        public ListResponse<ProductsPricesGetDto> GetMultiple() =>
            _productPriceManager.GetMultiple();

        [HttpPut]
        [Route("/products-prices")]
        public Response<long> Save(SaveProductPriceRequest request) =>
            _productPriceManager.Save(request);

        [HttpDelete]
        [Route("/products-prices/{id}")]
        public Response Delete([FromRoute]IdRequest request) =>
            _productPriceManager.Delete(request);
    }
}
