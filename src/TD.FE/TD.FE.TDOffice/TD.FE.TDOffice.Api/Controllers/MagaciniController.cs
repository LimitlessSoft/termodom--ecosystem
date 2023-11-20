using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class MagaciniController : ControllerBase
    {
        private readonly IMagacinManager _magacinManager;
        public MagaciniController(IMagacinManager magacinManager)
        {
            _magacinManager = magacinManager;
        }

        [HttpGet]
        [Route("/magacini")]
        public LSCoreListResponse<MagacinDto> GetMultiple()
        {
            return _magacinManager.GetMultiple();
        }
    }
}
