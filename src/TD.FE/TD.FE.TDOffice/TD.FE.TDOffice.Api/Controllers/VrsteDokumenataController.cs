using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.FE.TDOffice.Contracts.Dtos.VrsteDokumenata;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class VrsteDokumenataController : Controller
    {
        private readonly IVrstaDokumentaManager _vrstaDokumentaManager;
        public VrsteDokumenataController(IVrstaDokumentaManager vrstaDokumentaManager)
        {
            _vrstaDokumentaManager = vrstaDokumentaManager;
        }

        [HttpGet]
        [Route("/vrste-dokumenata")]
        public LSCoreListResponse<VrstaDokumentaDto> GetMultiple()
        {
            return _vrstaDokumentaManager.GetMultiple();
        }
    }
}
