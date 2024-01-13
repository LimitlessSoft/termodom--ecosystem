using LSCore.Contracts.Http;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface ITDWebAdminApiManager
    {
        Task<LSCoreResponse<List<ProductsGetDto>>> ProductsGetMultipleAsync();
        Task<LSCoreResponse<List<KomercijalnoWebProductLinksGetDto>>> KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync();
        Task<LSCoreResponse<KomercijalnoWebProductLinksGetDto>> KomercijalnoWebProductLinksControllerPutAsync(KomercijalnoWebProductLinksSaveRequest request);
    }
}
