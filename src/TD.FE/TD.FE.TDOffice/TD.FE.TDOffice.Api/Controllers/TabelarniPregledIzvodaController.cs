using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.TabelarniPregledIzvoda;
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
        public ListResponse<TabelarniPregledIzvodaGetDto> Get([FromQuery] TabelarniPregledIzvodaGetRequest request)
        {
            return _tabelarniPregledIzvodaManager.Get(request);
        }

        [HttpPut]
        [Route("/tabelarni-pregled-izvoda")]
        public Response<TabelarniPregledIzvodaGetDto> Put([FromBody] DokumentTagizvodPutRequest request)
        {
            return _tabelarniPregledIzvodaManager.Put(request);
        }
    }
}
