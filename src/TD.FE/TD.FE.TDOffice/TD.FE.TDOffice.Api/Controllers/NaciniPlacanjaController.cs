using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.FE.TDOffice.Contracts.Dtos.NaciniPlacanja;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class NaciniPlacanjaController : Controller
    {
        private readonly INacinPlacanjaManager _nacinPlacanjaManager;
        public NaciniPlacanjaController(INacinPlacanjaManager nacinPlacanjaManger)
        {
            _nacinPlacanjaManager = nacinPlacanjaManger;
        }

        [HttpGet]
        [Route("/nacini-placanja")]
        public LSCoreListResponse<NacinPlacanjaDto> GetMultiple()
        {
            return _nacinPlacanjaManager.GetMultiple();
        }
    }
}
