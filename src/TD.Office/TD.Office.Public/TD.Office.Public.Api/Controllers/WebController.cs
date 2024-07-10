using Microsoft.AspNetCore.Authorization;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Office.Public.Contracts.Requests.Web;
using TD.Web.Admin.Contracts.Dtos.Products;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Api.Controllers;

[Authorize]
[ApiController]
[Permissions(Permission.Access, Permission.WebRead)]
public class WebController (IWebManager webManager) : ControllerBase
{
    [HttpGet]
    [Route("/web-azuriranje-cena")]
    public async Task<IActionResult> AzuriranjeCenaAsync([FromQuery] WebAzuiranjeCenaRequest request) =>
        Ok(await webManager.AzuriranjeCenaAsync(request));

    [HttpPost]
    [Route("/web-azuriraj-cene-max-web-osnove")]
    public async Task AzurirajCeneMaxWebOsnove([FromBody] ProductsUpdateMaxWebOsnoveRequest request) =>
        await webManager.AzurirajCeneMaxWebOsnove(request);

    [HttpPost]
    [Route("/web-azuriraj-cene-min-web-osnove")]
    public async Task AzurirajCeneMinWebOsnove() =>
        await webManager.AzurirajCeneMinWebOsnove();

    [HttpPost]
    [Route("/web-azuriraj-cene-komercijalno-poslovanje")]
    public async Task AzurirajCeneKomercijalnoPoslovajne() =>
        await webManager.AzurirajCeneKomercijalnoPoslovajne();

    [HttpPut]
    [Route("/web-azuriraj-cene-komercijalno-poslovanje-povezi-proizvode")]
    public async Task<KomercijalnoWebProductLinksGetDto?> AzurirajCeneKomercijalnoPoslovajnePoveziProizvode([FromBody] KomercijalnoWebProductLinksSaveRequest request) =>
        await webManager.AzurirajCeneKomercijalnoPoslovanjePoveziProizvode(request);

    [HttpPut]
    [Route("/web-azuriraj-cene-uslovi-formiranja-min-web-osnova")]
    public void AzurirajCeneUsloviFormiranjaMinWebOsnova([FromBody] WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest request) =>
        webManager.AzurirajCeneUsloviFormiranjaMinWebOsnova(request);
        
    [HttpGet]
    [Route("/web-azuriraj-cene-uslov-formiranja-min-web-osnova-product-suggestion")]
    public Task<List<KeyValuePair<long, string>>> AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestion([FromQuery] AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionRequest request) =>
        webManager.AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestion(request);
        
    [HttpGet]
    [Route("/web-products")]
    public Task<List<ProductsGetDto>?> GetProducts([FromQuery] ProductsGetMultipleRequest request) =>
        webManager.GetProducts(request);
}