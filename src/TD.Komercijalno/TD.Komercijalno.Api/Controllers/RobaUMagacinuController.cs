using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class RobaUMagacinuController : Controller
    {
        private readonly IRobaUMagacinuManager _robaUMagacinuManager;
        public RobaUMagacinuController(IRobaUMagacinuManager robaUMagacinuManager)
        {
            _robaUMagacinuManager = robaUMagacinuManager;
        }

        [HttpGet]
        [Route("/roba-u-magacinu")]
        public LSCoreListResponse<RobaUMagacinuGetDto> GetMultiple([FromQuery] RobaUMagacinuGetMultipleRequest request) =>
            _robaUMagacinuManager.GetMultiple(request);
    }
}
