using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class DokumentController : Controller
    {
        private readonly IDokumentManager _dokumentManager;

        public DokumentController(IDokumentManager dokumentManager)
        {
            _dokumentManager = dokumentManager;
        }

        [HttpGet]
        [Route("/dokumenti")]
        public ListResponse<DokumentDto> GetMultiple([FromQuery] DokumentGetMultipleRequest request)
        {
            return _dokumentManager.GetMultiple(request);
        }

        [HttpPost("/dokumenti")]
        public Response<DokumentDto> Create([FromBody] DokumentCreateRequest request)
        {
            return _dokumentManager.Create(request);
        }
    }
}
