using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.IManagers;

namespace TD.Komercijalno.Api.Controllers
{
    [ApiController]
    public class NameneController : Controller
    {
        private readonly INamenaManager _namenaManager;
        public NameneController(INamenaManager namenaManager)
        {
            _namenaManager = namenaManager;
        }

        [HttpGet]
        [Route("/namene")]
        public LSCoreListResponse<NamenaDto> GetMultiple()
        {
            return _namenaManager.GetMultiple();
        }
    }
}
