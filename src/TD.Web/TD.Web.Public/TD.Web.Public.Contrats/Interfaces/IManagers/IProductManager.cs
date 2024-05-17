using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Responses;
using TD.Web.Public.Contracts.Dtos.Products;
using TD.Web.Public.Contracts.Requests.Products;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface IProductManager : ILSCoreBaseManager
    {
        LSCoreSortedPagedResponse<ProductsGetDto> GetMultiple(ProductsGetRequest request);
        Task<LSCoreFileResponse> GetImageForProductAsync(ProductsGetImageRequest request);
        LSCoreResponse<ProductsGetSingleDto> GetSingle(ProductsGetImageRequest request);
        LSCoreResponse<string> AddToCart(AddToCartRequest request);
        LSCoreResponse RemoveFromCart(RemoveFromCartRequest request);
        LSCoreResponse SetProductQuantity(SetCartQuantityRequest request);
        LSCoreListResponse<ProductsGetDto> GetFavorites();
    }
}
