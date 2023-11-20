using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using TD.TDOffice.Contracts.Dtos.Users;
using TD.TDOffice.Contracts.IManagers;

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
        public LSCoreResponse<UserDto> Get([FromRoute]LSCoreIdRequest request)
        {
            return _userManager.Get(request);
        }

        [HttpGet]
        [Route("/users")]
        public LSCoreListResponse<UserDto> GetMultiple()
        {
            return _userManager.GetMultiple();
        }
    }
}
