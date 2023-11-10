using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.IManagers;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class MagaciniController : Controller
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
