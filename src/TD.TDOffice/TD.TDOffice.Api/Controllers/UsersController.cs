using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Http.Interfaces;
using TD.Core.Contracts.Requests;
using TD.TDOffice.Contracts.Dtos.Users;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Domain.Managers;

namespace TD.TDOffice.Api.Controllers
{
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Route("/users/{id}")]
        public Response<UserDto> Get([FromRoute]IdRequest request)
        {
            return _userManager.Get(request);
        }

        [HttpGet]
        [Route("/users")]
        public ListResponse<UserDto> GetMultiple()
        {
            return _userManager.GetMultiple();
        }
    }
}
