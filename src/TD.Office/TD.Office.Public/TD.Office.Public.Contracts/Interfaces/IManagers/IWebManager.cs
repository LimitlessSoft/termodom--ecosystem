using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Office.Public.Contracts.Requests.Web;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Office.Public.Contracts.Dtos.Web;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IWebManager
{
    Task<List<WebAzuriranjeCenaDto>> AzuriranjeCenaAsync(WebAzuiranjeCenaRequest request);
    Task AzurirajCeneKomercijalnoPoslovajne();
    Task<KomercijalnoWebProductLinksGetDto?> AzurirajCeneKomercijalnoPoslovanjePoveziProizvode(KomercijalnoWebProductLinksSaveRequest request);
    void AzurirajCeneUsloviFormiranjaMinWebOsnova(WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest request);
    Task AzurirajCeneMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request);
    Task AzurirajCeneMinWebOsnove();
    Task<List<KeyValuePair<long, string>>> AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestion(AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionRequest request);
    Task<List<ProductsGetDto>?> GetProducts(ProductsGetMultipleRequest request);
}