using TD.Core.Contracts.Http;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Requests.Products;

namespace TD.Web.Contracts.Interfaces.Managers
{
    public interface IProductManager
    {
        ListResponse<ProductsGetMultipleDto> GetMultiple(ProductsGetMultipleRequest request);
        ListResponse<ProductsGetMultipleDto> GetSearch(ProductsGetSearchRequest request);
    }
}
