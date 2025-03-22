using LSCore.SortAndPage.Contracts;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contracts.Requests.Products;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IProductManager
{
	Task<LSCoreSortedAndPagedResponse<ProductsGetDto>> GetMultipleAsync(ProductsGetRequest request);

	// Task<LSCoreFileResponse> GetImageForProductAsync(ProductsGetImageRequest request);
	Task<ProductsGetSingleDto> GetSingleAsync(ProductsGetImageRequest request);
	string AddToCart(AddToCartRequest request);
	void RemoveFromCart(RemoveFromCartRequest request);
	void SetProductQuantity(SetCartQuantityRequest request);
	Task<LSCoreSortedAndPagedResponse<ProductsGetDto>> GetFavoritesAsync();
	Task<LSCoreSortedAndPagedResponse<ProductsGetDto>> GetSuggestedAsync(
		GetSuggestedProductsRequest request
	);
}
