using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Dtos.Units;
using TD.Web.Contracts.Interfaces.IManagers;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class UnitsController : Controller
    {
        private readonly IUnitManager _unitManager;

        public UnitsController(IUnitManager unitManager)
        {
            _unitManager = unitManager;
        }

        [HttpGet]
        [Route("/units/{id}")]
        public Response<UnitsGetDto> Get([FromRoute] int id)
        {
            return _unitManager.Get(new IdRequest(id));
        }
    }
}
