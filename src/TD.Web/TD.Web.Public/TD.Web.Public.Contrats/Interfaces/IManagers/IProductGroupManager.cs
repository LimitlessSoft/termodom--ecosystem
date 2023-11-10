using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Public.Contrats.Requests.ProductsGroups;

namespace TD.Web.Public.Contrats.Interfaces.IManagers
{
    public interface IProductGroupManager : ILSCoreBaseManager
    {
        LSCoreListResponse<LSCoreIdNamePairDto> GetMultiple(ProductsGroupsGetRequest request);
    }
}
