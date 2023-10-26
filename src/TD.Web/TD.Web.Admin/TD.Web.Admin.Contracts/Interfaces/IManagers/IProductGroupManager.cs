using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IProductGroupManager : IBaseManager
    {
        ListResponse<ProductsGroupsGetDto> GetMultiple();
        Response<ProductsGroupsGetDto> Get(IdRequest request);
        Response<long> Save(ProductsGroupsSaveRequest request);
    }
}
