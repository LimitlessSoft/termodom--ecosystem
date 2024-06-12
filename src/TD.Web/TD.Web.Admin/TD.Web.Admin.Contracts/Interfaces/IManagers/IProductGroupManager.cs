using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IProductGroupManager
{
    List<ProductsGroupsGetDto> GetMultiple();
    ProductsGroupsGetDto Get(LSCoreIdRequest request);
    long Save(ProductsGroupsSaveRequest request);
    void Delete(ProductsGroupsDeleteRequest request);
    void UpdateType(ProductsGroupUpdateTypeRequest request);
}