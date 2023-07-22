using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TD.Core.Contracts.Http;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Api.Controllers
{
    [ApiController]
    public class DokumentTagIzvodController : Controller
    {
        private readonly IDokumentTagIzvodManager _dokumentTagIzvodManager;

        public DokumentTagIzvodController(IDokumentTagIzvodManager dokumentTagIzvodManager)
        {
            _dokumentTagIzvodManager = dokumentTagIzvodManager;
        }

        [HttpGet]
        [Route("/dokument-tag-izvodi")]
        public ListResponse<DokumentTagIzvodGetDto> GetMultiple([FromQuery] DokumentTagIzvodGetMultipleRequest request)
        {
            return _dokumentTagIzvodManager.GetMultiple(request);
        }


        /// <summary>
        /// Insert ili update nad entitetom DokumentTagIzvod.
        /// Ukoliko se radi Update, BrojDokumentaIzvoda ce biti ignorisan iz requesta (nece biti azuiraran),
        /// u suprotnom novi entitet ce imati vrednost iz request.BrojDokumentaIzvoda
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/dokument-tag-izvodi")]
        public Response<DokumentTagIzvodGetDto> Put([FromBody] DokumentTagizvodPutRequest request)
        {
            return _dokumentTagIzvodManager.Save(request);
        }
    }
}
