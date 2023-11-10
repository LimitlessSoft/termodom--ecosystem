using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class NameneDokumentaController : Controller
    {
        private readonly INamenaDokumentaManager _namenaDokumentaManager;
        public NameneDokumentaController(INamenaDokumentaManager namenaDokumentaManager)
        {
            _namenaDokumentaManager = namenaDokumentaManager;
        }

        [HttpGet]
        [Route("/namene-dokumenta")]
        public LSCoreListResponse<NamenaDto> GetMultiple()
        {
            return _namenaDokumentaManager.GetMultiple();
        }
    }
}
