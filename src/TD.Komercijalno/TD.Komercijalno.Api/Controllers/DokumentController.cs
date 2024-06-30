using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class DokumentController (IDokumentManager dokumentManager) : Controller
    {
        [HttpGet]
        [Route("/dokumenti/{VrDok}/{BrDok}")]
        public IActionResult Get([FromRoute]DokumentGetRequest request) =>
            Ok(dokumentManager.Get(request));

        [HttpGet]
        [Route("/dokumenti")]
        public IActionResult GetMultiple([FromQuery] DokumentGetMultipleRequest request) =>
            Ok(dokumentManager.GetMultiple(request));

        [HttpPost("/dokumenti")]
        public IActionResult Create([FromBody] DokumentCreateRequest request) =>
            Ok(dokumentManager.Create(request));

        [HttpPut("/dokumenti/{VrDok}/{BrDok}/nacin-placanja/{NUID}")]
        public IActionResult SetNacinPlacanja([FromRoute] DokumentSetNacinPlacanjaRequest request)
        {
            dokumentManager.SetNacinPlacanja(request);
            return Ok();
        }
    }
}
