using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Configuration;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;

namespace TD.Office.Public.Domain.Managers
{
    public class TDWebAdminApiManager : LSCoreBaseApiManager, ITDWebAdminApiManager
    {
        public TDWebAdminApiManager(IConfigurationRoot configurationRoot)
        {
            HttpClient.BaseAddress = new Uri(configurationRoot["TD_WEB_API_URL"]!);
        }

        public Task<LSCoreResponse<List<ProductsGetDto>>> ProductsGetMultipleAsync() =>
            GetAsync<List<ProductsGetDto>>("/products");

        public Task<LSCoreResponse> ProductsUpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request) =>
            PutAsync<ProductsUpdateMaxWebOsnoveRequest>("/products-update-max-web-osnove", request);

        public Task<LSCoreResponse<List<KomercijalnoWebProductLinksGetDto>>> KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync() =>
            GetAsync<List<KomercijalnoWebProductLinksGetDto>>("/komercijalno-web-product-links");

        public Task<LSCoreResponse<KomercijalnoWebProductLinksGetDto>> KomercijalnoWebProductLinksControllerPutAsync(KomercijalnoWebProductLinksSaveRequest request) =>
            PutAsync<KomercijalnoWebProductLinksSaveRequest, KomercijalnoWebProductLinksGetDto>("/komercijalno-web-product-links", request);
    }
}
