using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class VrstaDokController (IVrstaDokManager vrstaDokManager) : Controller
    {
        [HttpGet]
        [Route("/vrste-dokumenata")]
        public List<VrstaDokDto> GetMultiple() =>
            vrstaDokManager.GetMultiple();
    }
}
