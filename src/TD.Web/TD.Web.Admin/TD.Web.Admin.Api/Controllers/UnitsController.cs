using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Interfaces.Managers;
using TD.Web.Admin.Contracts.Requests.Units;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class UnitsController : ControllerBase
    {
        private readonly IUnitManager _unitManager;

        public UnitsController(IUnitManager unitManager, IHttpContextAccessor httpContextAccessor)
        {
            _unitManager = unitManager;
            _unitManager.SetContextInfo(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("/units/{id}")]
        public Response<UnitsGetDto> Get([FromRoute] int id)
        {
            return _unitManager.Get(new IdRequest(id));
        }

        [HttpGet]
        [Route("/units")]
        public ListResponse<UnitsGetDto> GetMultiple()
        {
            return _unitManager.GetMultiple();
        }

        [HttpPut]
        [Route("/units")]
        public Response<long> Save([FromBody] UnitSaveRequest request)
        {
            return _unitManager.Save(request);
        }

        [HttpDelete]
        [Route("/units/{Id}")]
        public Response Delete([FromRoute] IdRequest request)
        {
            return _unitManager.Delete(request);
        }
    }
}
