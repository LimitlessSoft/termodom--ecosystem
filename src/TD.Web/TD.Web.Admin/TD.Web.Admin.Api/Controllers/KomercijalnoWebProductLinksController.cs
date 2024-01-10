using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class KomercijalnoWebProductLinksController : ControllerBase
    {
        private readonly IKomercijalnoWebProductLinkManager _komercijalnoWebProductLinkManager;
        public KomercijalnoWebProductLinksController(IKomercijalnoWebProductLinkManager komercijalnoWebProductLinkManager)
        {
            _komercijalnoWebProductLinkManager = komercijalnoWebProductLinkManager;
        }

        [HttpGet]
        [Route("/komercijalno-web-product-links")]
        public LSCoreListResponse<KomercijalnoWebProductLinksGetDto> GetMultiple() =>
            _komercijalnoWebProductLinkManager.GetMultiple();
    }
}
