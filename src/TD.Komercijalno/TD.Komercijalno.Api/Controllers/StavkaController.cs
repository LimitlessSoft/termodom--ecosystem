using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class StavkaController (IStavkaManager stavkaManager) : Controller
    {
        [HttpPost]
        [Route("/stavke")]
        public StavkaDto Create([FromBody] StavkaCreateRequest request) =>
            stavkaManager.Create(request);

        [HttpGet]
        [Route("/stavke")]
        public List<StavkaDto> GetMultiple(StavkaGetMultipleRequest request) =>
            stavkaManager.GetMultiple(request);

        [HttpDelete]
        [Route("/stavke")]
        public IActionResult DeleteStavke([FromQuery] StavkeDeleteRequest request)
        {
            stavkaManager.DeleteStavke(request);
            return Ok();
        }
    }
}
