using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Public.Contrats.Dtos.Users;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userManager.SetContext(httpContextAccessor.HttpContext);
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

        [HttpGet]
        [Route("/me")]
        public LSCoreResponse<UserInformationDto> Me()
        {
            return _userManager.Me();
        }
    }
}
