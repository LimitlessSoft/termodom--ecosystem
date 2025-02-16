using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Admin.Contracts.Dtos.Products;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Domain.Extensions;
using System.Net.Http.Json;
using LSCore.Contracts;

namespace TD.Office.Public.Domain.Managers
{
    public class TDWebAdminApiManager : ITDWebAdminApiManager
    {
        private readonly HttpClient _httpClient = new ();
        
        public TDWebAdminApiManager(IConfigurationRoot configurationRoot)
        {
            _httpClient.BaseAddress = new Uri(configurationRoot["TD_WEB_API_URL"]!);
            _httpClient.DefaultRequestHeaders.Add(LSCoreContractsConstants.ApiKeyCustomHeader, configurationRoot["TD_WEB_ADMIN_API_KEY"]!);
        }

        public async Task<List<ProductsGetDto>> ProductsGetMultipleAsync(ProductsGetMultipleRequest request)
        {
            var response = await _httpClient.GetAsync(
                $"/products?{(request.Id is null ? "" : string.Join('&', request.Id!.Select(z => "id=" + z)))}&searchFilter={request.SearchFilter}");
            
            response.HandleStatusCode();
            return (await response.Content.ReadFromJsonAsync<List<ProductsGetDto>>())!;
        }

        public async Task ProductsUpdateMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("/products-update-max-web-osnove", request);
            
            response.HandleStatusCode();;
        }

        public async Task<List<KomercijalnoWebProductLinksGetDto>> KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync()
        {
            var response = await _httpClient.GetAsync("/komercijalno-web-product-links");
            
            response.HandleStatusCode();
            
            return (await response.Content.ReadFromJsonAsync<List<KomercijalnoWebProductLinksGetDto>>())!;
        }

        public async Task<KomercijalnoWebProductLinksGetDto> KomercijalnoWebProductLinksControllerPutAsync(
            KomercijalnoWebProductLinksSaveRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("/komercijalno-web-product-links", request);
            
            response.HandleStatusCode();
            
            return (await response.Content.ReadFromJsonAsync<KomercijalnoWebProductLinksGetDto>())!;
        }

        public Task UpdateMinWebOsnove(ProductsUpdateMinWebOsnoveRequest request) =>
            _httpClient.PutAsJsonAsync("/products-update-min-web-osnove", request);
    }
}
