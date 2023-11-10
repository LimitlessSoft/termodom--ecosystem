using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.TabelarniPregledIzvoda;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

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
        public LSCoreListResponse<TabelarniPregledIzvodaGetDto> Get([FromQuery] TabelarniPregledIzvodaGetRequest request)
        {
            return _tabelarniPregledIzvodaManager.Get(request);
        }

        [HttpPut]
        [Route("/tabelarni-pregled-izvoda")]
        public LSCoreResponse<DokumentTagIzvodGetDto> Put([FromBody] DokumentTagizvodPutRequest request)
        {
            return _tabelarniPregledIzvodaManager.Put(request);
        }
    }
}
