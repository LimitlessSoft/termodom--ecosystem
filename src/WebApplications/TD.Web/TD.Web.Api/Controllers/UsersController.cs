using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Framework;
using TD.Web.Contracts.Enums;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Requests.Users;

namespace TD.Web.Api.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [Route("/login")]
        public Response<string> Login([FromBody]UserLoginRequest request)
        {
            return _userManager.Login(request);
        }

        [HttpPut]
        [Route("/register")]
        public Response Register([FromBody]UserRegisterRequest request)
        {
            return _userManager.Register(request);
        }

        [HttpPost]
        [Route("/user/{Id}/promote")]
        public Response<bool> PromoteUser(UserPromoteRequest request)
        {
            return _userManager.PromoteUser(request);
        }
    }
}
