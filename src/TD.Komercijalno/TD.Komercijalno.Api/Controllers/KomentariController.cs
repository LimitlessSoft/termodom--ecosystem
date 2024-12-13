using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class KomentariController (IKomentarManager komentarManager) : Controller
    {
        [HttpPost]
        [Route("/komentari")]
        public KomentarDto Create([FromBody] CreateKomentarRequest request) =>
            komentarManager.Create(request);

        [HttpPost]
        [Route("/komentari-flush")]
        public IActionResult FlushComments([FromBody] FlushCommentsRequest request) =>
            Ok(komentarManager.FlushComments(request));

        [HttpPut]
        [Route("/komentari")]
        public KomentarDto Update([FromBody] UpdateKomentarRequest request) =>
            komentarManager.Update(request);
    }
}
