using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Configuration;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoPrices;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.Products;

namespace TD.Office.Public.Domain.Managers
{
    public class TDWebAdminApiManager : LSCoreBaseApiManager, ITDWebAdminApiManager
    {
        public TDWebAdminApiManager(IConfigurationRoot configurationRoot)
        {
            HttpClient.BaseAddress = new Uri(configurationRoot["TD_WEB_API_URL"]!);
        }

        public Task<LSCoreResponse<List<KomercijalnoWebProductLinksGetDto>>> KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync() =>
            base.GetAsync<List<KomercijalnoWebProductLinksGetDto>>("/komercijalno-web-product-links");

        public Task<LSCoreResponse<List<KomercijalnoPriceGetDto>>> KomercijalnoPricesGetMultipleAsync() =>
            base.GetAsync<List<KomercijalnoPriceGetDto>>("/komercijalno-prices");

        public Task<LSCoreResponse<List<Web.Admin.Contracts.Dtos.Products.ProductsGetDto>>> ProductsGetMultipleAsync() =>
            base.GetAsync<List<ProductsGetDto>>("/products");
    }
}
