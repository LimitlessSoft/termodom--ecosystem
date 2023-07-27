using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class WebUredjivanjeProizvodaController : Controller
    {
        [HttpGet]
        [Route("/web-uredjivanje-proizvoda/komercijalno-roba")]
        public Response Index()
        {
            return new Response();
        }
    }
}
