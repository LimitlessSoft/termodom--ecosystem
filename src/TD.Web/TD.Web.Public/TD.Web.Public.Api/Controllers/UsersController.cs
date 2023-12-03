using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("/login")]
        public LSCoreResponse<string> Login([FromBody] UserLoginRequest request)
        {
            return _userManager.Login(request);
        }

        [HttpPut]
        [Route("/register")]
        public LSCoreResponse Register([FromBody] UserRegisterRequest request)
        {
            return _userManager.Register(request);
        }
    }
}
