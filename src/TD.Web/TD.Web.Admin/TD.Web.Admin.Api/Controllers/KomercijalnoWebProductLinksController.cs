using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class KomercijalnoWebProductLinksController (
    IKomercijalnoWebProductLinkManager komercijalnoWebProductLinkManager)
    : ControllerBase
{
    [HttpGet]
    [Route("/komercijalno-web-product-links")]
    public List<KomercijalnoWebProductLinksGetDto> GetMultiple() =>
        komercijalnoWebProductLinkManager.GetMultiple();

    [HttpPut]
    [Route("/komercijalno-web-product-links")]
    public KomercijalnoWebProductLinksGetDto Put(KomercijalnoWebProductLinksSaveRequest request) =>
        komercijalnoWebProductLinkManager.Put(request);
}