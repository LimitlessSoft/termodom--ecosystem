using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Web;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;

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
        [Route("/web-azuriraj-cene-max-web-osnove")]
        public async Task<LSCoreResponse> AzurirajCeneMaxWebOsnove([FromBody] ProductsUpdateMaxWebOsnoveRequest request) =>
            await _webManager.AzurirajCeneMaxWebOsnove(request);

        [HttpPost]
        [Route("/web-azuriraj-cene-min-web-osnove")]
        public async Task<LSCoreResponse> AzurirajCeneMinWebOsnove() =>
            await _webManager.AzurirajCeneMinWebOsnove();

        [HttpPost]
        [Route("/web-azuriraj-cene-komercijalno-poslovanje")]
        public async Task<LSCoreResponse> AzurirajCeneKomercijalnoPoslovajne() =>
            await _webManager.AzurirajCeneKomercijalnoPoslovajne();

        [HttpPut]
        [Route("/web-azuriraj-cene-komercijalno-poslovanje-povezi-proizvode")]
        public async Task<LSCoreResponse<KomercijalnoWebProductLinksGetDto>> AzurirajCeneKomercijalnoPoslovajnePoveziProizvode([FromBody] KomercijalnoWebProductLinksSaveRequest request) =>
            await _webManager.AzurirajCeneKomercijalnoPoslovajnePoveziProizvode(request);

        [HttpPut]
        [Route("/web-azuriraj-cene-uslovi-formiranja-min-web-osnova")]
        public LSCoreResponse AzurirajCeneUsloviFormiranjaMinWebOsnova([FromBody] WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest request) =>
            _webManager.AzurirajCeneUsloviFormiranjaMinWebOsnova(request);
    }
}
