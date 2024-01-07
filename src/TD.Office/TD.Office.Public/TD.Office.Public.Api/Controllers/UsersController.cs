using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers
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
        public LSCoreResponse<string> Login([FromBody] UsersLoginRequest request) =>
            _userManager.Login(request);
    }
}
