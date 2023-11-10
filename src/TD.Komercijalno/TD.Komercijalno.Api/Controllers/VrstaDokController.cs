using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.IManagers;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class VrstaDokController : Controller
    {
        private readonly IVrstaDokManager _vrstaDokManager;
        public VrstaDokController(IVrstaDokManager vrstaDokManager)
        {
            _vrstaDokManager = vrstaDokManager;
        }

        [HttpGet]
        [Route("/vrste-dokumenata")]
        public LSCoreListResponse<VrstaDokDto> GetMultiple()
        {
            return _vrstaDokManager.GetMultiple();
        }
    }
}
