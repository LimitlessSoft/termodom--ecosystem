using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Products;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class ProductsController(IProductManager productManager) : ControllerBase
{
    [HttpGet]
    [Route("/products")]
    public async Task<LSCoreSortedAndPagedResponse<ProductsGetDto>> GetMultiple(
        [FromQuery] ProductsGetRequest request
    ) => await productManager.GetMultipleAsync(request);

    // [HttpGet]
    // [Route("/products/{Src}/image")]
    // public Task<LSCoreFileResponse> GetImageForProductAsync([FromRoute] string Src, [FromQuery] ProductsGetImageRequest request)
    // {
    //     request.Src = Src;
    //     return _productManager.GetImageForProductAsync(request);
    // }

    [HttpGet]
    [Route("/products/{Src}")]
    public async Task<ProductsGetSingleDto> GetSingle(
        [FromRoute] ProductsGetImageRequest request
    ) => await productManager.GetSingleAsync(request);

    [HttpPut]
    [Route("/products/{id}/add-to-cart")]
    public string AddToCart([FromRoute] long id, [FromBody] AddToCartRequest request)
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
    public IActionResult GetFavorites() => Ok(productManager.GetFavoritesAsync());

    [HttpGet]
    [Route("/suggested-products")]
    public async Task<IActionResult> GetSuggested([FromQuery] GetSuggestedProductsRequest request) =>
        Ok(await productManager.GetSuggestedAsync(request));
}
