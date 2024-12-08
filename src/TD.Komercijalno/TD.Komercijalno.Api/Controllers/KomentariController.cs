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
        public KomentarDto Create(CreateKomentarRequest request) =>
            komentarManager.Create(request);

        [HttpPost]
        [Route("/komentari/flush")]
        public void FlushComments(FlushCommentsRequest request) =>
            komentarManager.FlushComments(request);
    }
}
