using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Requests.Products;

namespace TD.Web.Admin.Contracts.Interfaces.Managers
{
    public interface IProductManager
    {
        Response<ProductsGetDto> Get(IdRequest request);
        ListResponse<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request);
        ListResponse<ProductsGetDto> GetSearch(ProductsGetSearchRequest request);
        Response<long> Save(ProductsSaveRequest request);
        ListResponse<IdNamePairDto> GetClassifications();
    }
}
