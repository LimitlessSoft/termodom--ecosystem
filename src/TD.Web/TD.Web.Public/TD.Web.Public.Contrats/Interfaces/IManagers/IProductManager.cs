using TD.Web.Public.Contracts.Requests.Products;
using TD.Web.Public.Contracts.Dtos.Products;
using LSCore.Contracts.Responses;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IProductManager
{
    LSCoreSortedAndPagedResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request);
    // Task<LSCoreFileResponse> GetImageForProductAsync(ProductsGetImageRequest request);
    ProductsGetSingleDto GetSingle(ProductsGetImageRequest request);
    string AddToCart(AddToCartRequest request);
    void RemoveFromCart(RemoveFromCartRequest request);
    void SetProductQuantity(SetCartQuantityRequest request);
    LSCoreSortedAndPagedResponse<ProductsGetDto> GetFavorites();
    LSCoreSortedAndPagedResponse<ProductsGetDto> GetSuggested(GetSuggestedProductsRequest request);
}