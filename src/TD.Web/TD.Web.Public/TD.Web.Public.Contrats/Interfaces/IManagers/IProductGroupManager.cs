using TD.Web.Public.Contracts.Requests.ProductsGroups;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IProductGroupManager
{
    List<ProductsGroupsGetDto> GetMultiple(ProductsGroupsGetRequest request);
    ProductsGroupsGetDto Get(string name);
}