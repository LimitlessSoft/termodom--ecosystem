using Demo.Contracts.Entities;
using Demo.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;

namespace Demo.Api.Controllers
{
    [ApiController]
    public class CarsController : ControllerBase
    {
        private readonly ICarManager _carManager;
        public CarsController(ICarManager carManager)
        {
            _carManager = carManager;
        }

        [HttpGet]
        [Route("/cars")]
        public ListResponse<CarEntity> Get()
        {
            return _carManager.Get();
        }

        [HttpGet]
        [Route("/cars/{id}")]
        public Response<CarEntity> Get([FromRoute] int id)
        {
            return _carManager.Get(new IdRequest(id));
        }
    }
}
