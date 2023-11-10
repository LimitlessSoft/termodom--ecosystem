using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class KomentariController : Controller
    {
        private readonly IKomentarManager _komentarManager;

        public KomentariController(IKomentarManager komentarManager)
        {
            _komentarManager = komentarManager;
        }

        [HttpPost]
        [Route("/komentari")]
        public LSCoreResponse<KomentarDto> Create(CreateKomentarRequest request)
        {
            return _komentarManager.Create(request);
        }
    }
}
