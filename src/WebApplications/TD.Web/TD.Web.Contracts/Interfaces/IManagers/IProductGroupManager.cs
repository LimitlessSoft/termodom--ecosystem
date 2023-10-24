using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.ProductsGroups;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IProductGroupManager : IBaseManager
    {
        ListResponse<ProductsGroupsGetDto> GetMultiple();
        Response<ProductsGroupsGetDto> Get(IdRequest request);

    }
}
