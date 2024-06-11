using TD.Komercijalno.Contracts.Requests.RobaUMagacinu;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class RobaUMagacinuController (IRobaUMagacinuManager robaUMagacinuManager) : Controller
    {
        [HttpGet]
        [Route("/roba-u-magacinu")]
        public List<RobaUMagacinuGetDto> GetMultiple([FromQuery] RobaUMagacinuGetMultipleRequest request) =>
            robaUMagacinuManager.GetMultiple(request);
    }
}
