using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class WebUredjivanjeProizvodaController : Controller
    {
        private readonly IWebUredjivanjeProizvodaManager _webUredjivanjeProizvodaManager;

        public WebUredjivanjeProizvodaController(IWebUredjivanjeProizvodaManager webUredjivanjeProizvodaManager)
        {
            _webUredjivanjeProizvodaManager = webUredjivanjeProizvodaManager;
        }

        [HttpGet]
        [Route("/web-uredjivanje-proizvoda/komercijalno-roba")]
        public Response<string> KomercijalnoRobaGet()
        {
            return _webUredjivanjeProizvodaManager.KomercijalnoRobaGet();
        }
    }
}
