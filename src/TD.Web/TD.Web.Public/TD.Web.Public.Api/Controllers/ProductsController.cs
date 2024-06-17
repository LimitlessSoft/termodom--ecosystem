using LSCore.Contracts.Responses;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Contracts.Dtos.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class ProductsController (IProductManager productManager)
    : ControllerBase
{
    [HttpGet]
    [Route("/products")]
    public LSCoreSortedAndPagedResponse<ProductsGetDto> GetMultiple([FromQuery]ProductsGetRequest request) =>
        productManager.GetMultiple(request);

    // [HttpGet]
    // [Route("/products/{Src}/image")]
    // public Task<LSCoreFileResponse> GetImageForProductAsync([FromRoute] string Src, [FromQuery] ProductsGetImageRequest request)
    // {
    //     request.Src = Src;
    //     return _productManager.GetImageForProductAsync(request);
    // }

    [HttpGet]
    [Route("/products/{Src}")]
    public ProductsGetSingleDto GetSingle([FromRoute] ProductsGetImageRequest request) =>
        productManager.GetSingle(request);

    [HttpPut]
    [Route("/products/{id}/add-to-cart")]
    public string AddToCart([FromRoute]long id, [FromBody]AddToCartRequest request)
    {
        request.Id = id;
        return productManager.AddToCart(request);
    }

    [HttpDelete]
    [Route("/products/{id}/remove-from-cart")]
    public void RemoveFromCart([FromRoute] long id, [FromBody] RemoveFromCartRequest request)
    {
        request.Id = id;
        productManager.RemoveFromCart(request);
    }

    [HttpPut]
    [Route("/products/{id}/set-cart-quantity")]
    public void SetProductQuantity([FromRoute] long id, [FromBody] SetCartQuantityRequest request)
    {
        request.Id = id;
        productManager.SetProductQuantity(request);
    }

    [HttpGet]
    [Authorize]
    [Route("/favorite-products")]
    public IActionResult GetFavorites() =>
        Ok(productManager.GetFavorites());

    [HttpGet]
    [Route("/suggested-products")]
    public IActionResult GetSuggested([FromQuery] GetSuggestedProductsRequest request) =>
        Ok(productManager.GetSuggested(request));
}