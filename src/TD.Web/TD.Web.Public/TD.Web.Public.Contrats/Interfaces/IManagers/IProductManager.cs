using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Contracts.Dtos.Products;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IProductManager
{
    List<ProductsGetDto> GetMultiple(ProductsGetRequest request);
    // Task<LSCoreFileResponse> GetImageForProductAsync(ProductsGetImageRequest request);
    ProductsGetSingleDto GetSingle(ProductsGetImageRequest request);
    string AddToCart(AddToCartRequest request);
    void RemoveFromCart(RemoveFromCartRequest request);
    void SetProductQuantity(SetCartQuantityRequest request);
    List<ProductsGetDto> GetFavorites();
    List<ProductsGetDto> GetSuggested(GetSuggestedProductsRequest request);
}