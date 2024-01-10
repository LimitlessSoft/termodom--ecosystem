using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Web;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class WebController : ControllerBase
    {
        private readonly IWebManager _webManager;
        public WebController(IWebManager webManager)
        {
            _webManager = webManager;
        }

        [HttpGet]
        [Route("/web-azuriranje-cena")]
        public async Task<LSCoreSortedPagedResponse<WebAzuriranjeCenaDto>> AzuriranjeCenaAsync([FromQuery] WebAzuiranjeCenaRequest request) =>
            await _webManager.AzuriranjeCenaAsync(request);

        [HttpPost]
        [Route("/web-azuriraj-cene-komercijalno-poslovanje")]
        public async Task<LSCoreResponse> AzurirajCeneKomercijalnoPoslovajne() =>
            await _webManager.AzurirajCeneKomercijalnoPoslovajne();
    }
}
