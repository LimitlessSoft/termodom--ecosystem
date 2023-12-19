using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contrats.Requests.Products;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Requests.Products;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Extensions;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IProductManager _productManager;

        public ProductsController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpGet]
        [Route("/products")]
        public LSCoreListResponse<ProductsGetDto> GetMultiple([FromQuery]ProductsGetRequest request) =>
            _productManager.GetMultiple(request);

        [HttpGet]
        [Route("/products/{Src}/image")]
        public Task<LSCoreFileResponse> GetImageForProductAsync([FromRoute]ProductsGetImageRequest request) => 
            _productManager.GetImageForProductAsync(request);

        [HttpGet]
        [Route("/products/{src}")]
        public LSCoreResponse<ProductsGetSingleDto> GetSingle([FromRoute] ProductsGetImageRequest request) =>
            _productManager.GetSingle(request);

        [HttpPut]
        [Route("/products/{id}/add-to-cart")]
        public LSCoreResponse AddToCart([FromRoute]int id, [FromBody]AddToCartRequest request)
        {
            if (request.IdsNotMatch(id))
                return LSCoreResponse.BadRequest();
            return _productManager.AddToCart(request);
        }
    }
}
