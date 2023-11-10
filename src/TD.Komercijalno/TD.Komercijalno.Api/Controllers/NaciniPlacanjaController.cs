using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.IManagers;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class NaciniPlacanjaController : Controller
    {
        private readonly INacinPlacanjaManager _nacinPlacanjaManager;
        public NaciniPlacanjaController(INacinPlacanjaManager nacinPlacanjaManager)
        {
            _nacinPlacanjaManager = nacinPlacanjaManager;
        }

        [HttpGet]
        [Route("/nacini-placanja")]
        public LSCoreListResponse<NacinPlacanjaDto> GetMultiple()
        {
            return _nacinPlacanjaManager.GetMultiple();
        }
    }
}
