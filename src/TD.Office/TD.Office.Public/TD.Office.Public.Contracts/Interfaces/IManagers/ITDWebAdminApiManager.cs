using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Admin.Contracts.Dtos.Products;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ITDWebAdminApiManager
{
    Task<List<ProductsGetDto>> ProductsGetMultipleAsync(ProductsGetMultipleRequest request);
    Task<List<KomercijalnoWebProductLinksGetDto>> KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync();
    Task<KomercijalnoWebProductLinksGetDto> KomercijalnoWebProductLinksControllerPutAsync(KomercijalnoWebProductLinksSaveRequest request);
    Task ProductsUpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request);
    Task UpdateMinWebOsnove(ProductsUpdateMinWebOsnoveRequest request);
}