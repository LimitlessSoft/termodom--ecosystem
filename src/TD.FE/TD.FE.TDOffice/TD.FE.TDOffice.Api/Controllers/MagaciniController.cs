using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.Advanced;
using TD.Core.Contracts.Http;
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
        public ListResponse<MagacinDto> GetMultiple()
        {
            return _magacinManager.GetMultiple();
        }
    }
}
