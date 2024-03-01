using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Professions;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class ProfessionsController : ControllerBase
    {
        private readonly IProfessionManager _professionManager;

        public ProfessionsController(IProfessionManager professionManager)
        {
            _professionManager = professionManager;
        }

        [HttpGet]
        [Route("/professions")]
        public LSCoreListResponse<ProfessionsGetMultipleDto> GetMultiple() =>
            _professionManager.GetMultiple();

        [HttpPut]
        [Route("/professions")]
        public LSCoreResponse<long> Save([FromBody] SaveProfessionRequest request) =>
            _professionManager.Save(request);
    }
}
