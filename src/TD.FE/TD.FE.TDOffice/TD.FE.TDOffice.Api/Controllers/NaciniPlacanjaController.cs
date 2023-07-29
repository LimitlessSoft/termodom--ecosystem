using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
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
        public ListResponse<NacinPlacanjaDto> GetMultiple()
        {
            return _nacinPlacanjaManager.GetMultiple();
        }
    }
}
