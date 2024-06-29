using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class MagaciniController (IMagacinManager magacinManager) : Controller
    {
        [HttpGet]
        [Route("/magacini")]
        public List<MagacinDto> GetMultiple() =>
            magacinManager.GetMultiple();
    }
}
