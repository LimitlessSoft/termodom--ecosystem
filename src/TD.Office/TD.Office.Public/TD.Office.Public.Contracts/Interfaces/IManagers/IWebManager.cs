using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Requests.Web;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IWebManager
    {
        Task<LSCoreSortedPagedResponse<WebAzuriranjeCenaDto>> AzuriranjeCenaAsync(WebAzuiranjeCenaRequest request);
        Task<LSCoreResponse> AzurirajCeneKomercijalnoPoslovajne();
        Task<LSCoreResponse<KomercijalnoWebProductLinksGetDto>> AzurirajCeneKomercijalnoPoslovajnePoveziProizvode(KomercijalnoWebProductLinksSaveRequest request);
        LSCoreResponse AzurirajCeneUsloviFormiranjaMinWebOsnova(WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest request);
        Task<LSCoreResponse> AzurirajCeneMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request);
        Task<LSCoreResponse> AzurirajCeneMinWebOsnove();
        LSCoreResponse<KeyValuePair<int, string>> AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestion(AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionRequest request);
    }
}
