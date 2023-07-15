using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;
using TD.Core.Contracts.Http;
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
        public ListResponse<DokumentTagIzvod> GetMultiple([FromQuery] DokumentTagIzvodGetMultipleRequest request)
        {
            return _dokumentTagIzvodManager.GetMultiple(request);
        }

        [HttpPost]
        [Route("/dokument-tag-izvodi")]
        public Response<bool> Put([FromBody] DokumentTagizvodPutRequest request)
        {
            return _dokumentTagIzvodManager.Save(request);
        }
    }
}
