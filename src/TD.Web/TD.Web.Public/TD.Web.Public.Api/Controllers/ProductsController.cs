using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contrats.Dtos.Products;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contrats.Requests.Products;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using LSCore.Contracts.Responses;
using TD.Web.Public.Contracts.Requests.Products;
using LSCore.Contracts.Extensions;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductManager _productManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductsController(IProductManager productManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

            _productManager = productManager;
            _productManager.SetContext(_httpContextAccessor.HttpContext!);
        }

        [HttpGet]
        [Route("/products")]
        public LSCoreSortedPagedResponse<ProductsGetDto> GetMultiple([FromQuery]ProductsGetRequest request) =>
            _productManager.GetMultiple(request);

        [HttpGet]
        [Route("/products/{Src}/image")]
        public Task<LSCoreFileResponse> GetImageForProductAsync([FromRoute]ProductsGetImageRequest request) => 
            _productManager.GetImageForProductAsync(request);

        [HttpGet]
        [Route("/products/{Src}")]
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

        [HttpDelete]
        [Route("/products/{id}/remove-from-cart")]
        public LSCoreResponse RemoveFromCart([FromRoute] int id, [FromBody] RemoveFromCartRequest request)
        {
            if (request.IdsNotMatch(id))
                return LSCoreResponse.BadRequest();
            return _productManager.RemoveFromCart(request);
        }
    }
}
