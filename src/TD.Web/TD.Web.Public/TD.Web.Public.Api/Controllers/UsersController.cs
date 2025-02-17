using LSCore.Contracts.IManagers;
using LSCore.Domain.Managers;
using LSCore.Framework.Attributes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class UsersController (IUserManager userManager, LSCoreAuthorizeManager lsCoreAuthorizeManager) : ControllerBase
{
    [HttpPost]
    [Route("/login")]
    public string Login([FromBody] UserLoginRequest request) =>
        lsCoreAuthorizeManager.Authorize(request.Username, request.Password).AccessToken;

    [HttpPut]
    [Route("/register")]
    public void Register([FromBody] UserRegisterRequest request) =>
        userManager.Register(request);

    [HttpGet]
    [Route("/me")]
    public UserInformationDto Me() =>
        userManager.Me();
        
    [HttpPost]
    [Route("/reset-password")]
    public void ResetPassword([FromBody] UserResetPasswordRequest request) =>
        userManager.ResetPassword(request);
        
    [HttpPut]
    [LSCoreAuthorize]
    [Route("/set-password")]
    public void SetPassword([FromBody] UserSetPasswordRequest request) =>
        userManager.SetPassword(request);
}