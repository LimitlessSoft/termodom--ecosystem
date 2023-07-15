using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class TabelarniPregledIzvodaController : Controller
    {
        private readonly ITabelarniPregledIzvodaManager _tabelarniPregledIzvodaManager;
        public TabelarniPregledIzvodaController(ITabelarniPregledIzvodaManager tabelarniPregledIzvodaManager)
        {
            _tabelarniPregledIzvodaManager = tabelarniPregledIzvodaManager;
        }

        [HttpGet]
        [Route("/tabelarni-pregled-izvoda")]
        public ListResponse<TabelarniPregledIzvodaGetDto> Get()
        {
            return _tabelarniPregledIzvodaManager.Get();
        }
    }
}
