using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IProductGroupManager : ILSCoreBaseManager
    {
        LSCoreListResponse<ProductsGroupsGetDto> GetMultiple();
        LSCoreResponse<ProductsGroupsGetDto> Get(LSCoreIdRequest request);
        LSCoreResponse<long> Save(ProductsGroupsSaveRequest request);
        LSCoreResponse Delete(ProductsGroupsDeleteRequest request);
        LSCoreResponse UpdateType(ProductsGroupUpdateTypeRequest request);
    }
}
