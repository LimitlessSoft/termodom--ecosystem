using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class RobaController (IRobaManager robaManager) : Controller
    {
        [HttpPost]
        [Route("/roba")]
        public Roba Create([FromBody] RobaCreateRequest request) =>
            robaManager.Create(request);

        [HttpGet]
        [Route("/roba")]
        public List<RobaDto> GetMultiple([FromQuery] RobaGetMultipleRequest request) =>
            robaManager.GetMultiple(request);
    }
}
