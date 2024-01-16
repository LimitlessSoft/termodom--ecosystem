using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Dtos.Users;

namespace TD.Web.Common.Api.Controllers
{
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

        [HttpGet]
        [Route("/me")]
        public LSCoreResponse<UserInformationDto> Me() =>
            _userManager.Me();
    }
}
