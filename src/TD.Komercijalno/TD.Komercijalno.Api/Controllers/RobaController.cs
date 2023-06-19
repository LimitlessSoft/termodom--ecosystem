using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Roba;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class RobaController : Controller
    {
        private readonly IRobaManager _robaManager;

        public RobaController(IRobaManager robaManager)
        {
            _robaManager = robaManager;
        }

        [HttpPost]
        [Route("/roba")]
        public Response<Roba> Create([FromBody] RobaCreateRequest request)
        {
            return _robaManager.Create(request);
        }

        [HttpGet]
        [Route("/roba")]
        public ListResponse<RobaDto> GetMultiple([FromQuery] RobaGetMultipleRequest request)
        {
            return _robaManager.GetMultiple(request);
        }
    }
}
