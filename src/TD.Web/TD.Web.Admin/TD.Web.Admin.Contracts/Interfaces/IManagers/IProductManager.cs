using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Admin.Contracts.Dtos.Products;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Dtos;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IProductManager
{
    ProductsGetDto Get(LSCoreIdRequest request);
    List<ProductsGetDto> GetMultiple(ProductsGetMultipleRequest request);
    List<ProductsGetDto> GetSearch(ProductsGetSearchRequest request);
    long Save(ProductsSaveRequest request);
    List<LSCoreIdNamePairDto> GetClassifications();
    void UpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request);
    void UpdateMinWebOsnove(ProductsUpdateMinWebOsnoveRequest request);
}