using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Public.Contrats.Requests.ProductsGroups;

namespace TD.Web.Public.Contrats.Interfaces.IManagers
{
    public interface IProductGroupManager : IBaseManager
    {
        ListResponse<IdNamePairDto> GetMultiple(ProductsGroupsGetRequest request);
    }
}
