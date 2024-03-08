using LSCore.Contracts.Http;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    [LSCoreAuthorization(UserType.Admin, UserType.SuperAdmin)]
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

        [HttpPut]
        [Route("/komercijalno-web-product-links")]
        public LSCoreResponse<KomercijalnoWebProductLinksGetDto> Put(KomercijalnoWebProductLinksSaveRequest request) =>
            _komercijalnoWebProductLinkManager.Put(request);
    }
}
