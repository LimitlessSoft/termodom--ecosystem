using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Interfaces.Managers;
using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitManager _unitManager;

        public UnitsController(IUnitManager unitManager, IHttpContextAccessor httpContextAccessor)
        {
            _unitManager = unitManager;
            _unitManager.SetContext(httpContextAccessor.HttpContext!);
        }

        [HttpGet]
        [Route("/units/{id}")]
        public LSCoreResponse<UnitsGetDto> Get([FromRoute] int id) =>
            _unitManager.Get(new LSCoreIdRequest() { Id = id });

        [HttpGet]
        [Route("/units")]
        public LSCoreListResponse<UnitsGetDto> GetMultiple() => 
            _unitManager.GetMultiple();

        [HttpPut]
        [Route("/units")]
        public LSCoreResponse<long> Save([FromBody] UnitSaveRequest request) =>
            _unitManager.Save(request);

        [HttpDelete]
        [Route("/units/{Id}")]
        public LSCoreResponse Delete([FromRoute] LSCoreIdRequest request) =>
            _unitManager.Delete(request);
    }
}
