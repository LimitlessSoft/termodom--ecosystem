using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Dtos.Users;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(IHttpContextAccessor httpContextAccessor, IUserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userManager.SetContext(_httpContextAccessor.HttpContext!);
        }

        [HttpPost]
        [Route("/login")]
        public LSCoreResponse<string> Login([FromBody] UsersLoginRequest request) =>
            _userManager.Login(request);

        [HttpGet]
        [Route("/me")]
        public LSCoreResponse<UserMeDto> Me() =>
            _userManager.Me();
    }
}
