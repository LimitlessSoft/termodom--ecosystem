using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Requests.ProductsGroups;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface IProductGroupManager : ILSCoreBaseManager
    {
        LSCoreListResponse<ProductsGroupsGetDto> GetMultiple(ProductsGroupsGetRequest request);
        LSCoreResponse<ProductsGroupsGetDto> Get(string name);
    }
}
