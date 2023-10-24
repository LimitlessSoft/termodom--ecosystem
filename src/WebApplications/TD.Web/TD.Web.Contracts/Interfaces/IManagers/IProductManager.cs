using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.Products;
using TD.Web.Contracts.Requests.Products;

namespace TD.Web.Contracts.Interfaces.Managers
{
    public interface IProductManager
    {
        Response<ProductsGetDto> Get(IdRequest request);
        ListResponse<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request);
        ListResponse<ProductsGetDto> GetSearch(ProductsGetSearchRequest request);
        Response<long> Save(ProductsSaveRequest request);
        ListResponse<ProductsClassificationsDto> GetClassifications();
    }
}
