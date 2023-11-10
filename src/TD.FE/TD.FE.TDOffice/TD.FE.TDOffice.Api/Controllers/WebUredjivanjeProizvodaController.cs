using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.FE.TDOffice.Contracts.Dtos.WebUredjivanjeProizvoda;
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
        [Route("/web-uredjivanje-proizvoda/proizvodi")]
        public async Task<LSCoreListResponse<WebUredjivanjeProizvodaProizvodiGetDto>> ProizvodiGet()
        {
            return await _webUredjivanjeProizvodaManager.ProizvodiGet();
        }
    }
}
