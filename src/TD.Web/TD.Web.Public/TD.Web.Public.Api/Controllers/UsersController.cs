using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userManager.SetContext(httpContextAccessor.HttpContext!);
        }

        [HttpPost]
        [Route("/login")]
        public LSCoreResponse<string> Login([FromBody] UserLoginRequest request) =>
            _userManager.Login(request);

        [HttpPut]
        [Route("/register")]
        public LSCoreResponse Register([FromBody] UserRegisterRequest request) =>
            _userManager.Register(request);

        [HttpGet]
        [Route("/me")]
        public LSCoreResponse<UserInformationDto> Me() =>
            _userManager.Me();
        
        [HttpPost]
        [Route("/reset-password")]
        public LSCoreResponse ResetPassword([FromBody] UserResetPasswordRequest request) =>
            _userManager.ResetPassword(request);
        
        [HttpPut]
        [Authorize]
        [Route("/set-password")]
        public LSCoreResponse SetPassword([FromBody] UserSetPasswordRequest request) =>
            _userManager.SetPassword(request);
    }
}
