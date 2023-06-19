using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class StavkaController : Controller
    {
        private readonly IStavkaManager _stavkaManager;

        public StavkaController(IStavkaManager stavkaManager)
        {
            _stavkaManager = stavkaManager;
        }

        [HttpPost]
        [Route("/stavke")]
        public Response<StavkaDto> Create([FromBody] StavkaCreateRequest request)
        {
            return _stavkaManager.Create(request);
        }
    }
}
