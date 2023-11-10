using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Requests.Products;

namespace TD.Web.Admin.Contracts.Interfaces.Managers
{
    public interface IProductManager : ILSCoreBaseManager
    {
        LSCoreResponse<ProductsGetDto> Get(LSCoreIdRequest request);
        LSCoreListResponse<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request);
        LSCoreListResponse<ProductsGetDto> GetSearch(ProductsGetSearchRequest request);
        LSCoreResponse<long> Save(ProductsSaveRequest request);
        LSCoreListResponse<LSCoreIdNamePairDto> GetClassifications();
    }
}
