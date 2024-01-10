using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Requests.Web;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IWebManager
    {
        Task<LSCoreSortedPagedResponse<WebAzuriranjeCenaDto>> AzuriranjeCenaAsync(WebAzuiranjeCenaRequest request);
        Task<LSCoreResponse> AzurirajCeneKomercijalnoPoslovajne();
    }
}
