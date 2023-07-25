using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Web.Veleprodaja.Contracts.Dtos.Users;
using TD.Web.Veleprodaja.Contracts.IManagers;
using TD.Web.Veleprodaja.Contracts.Requests;

namespace TD.Web.Veleprodaja.Api.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserManager _userManager;

        public UsersController(IUserManager userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("~/authenticate")]
        public Response<string> Authenticate([FromBody]UsersAuthenticateRequest request)
        {
            return _userManager.Authenticate(request);
        }

        [HttpGet]
        [Authorize]
        [Route("~/me")]
        public Response<UsersMeDto> Me()
        {
            _userManager.SetContext(HttpContext);
            return _userManager.Me();
        }
    }
}
