using LSCore.Auth.Contracts;
using LSCore.Auth.UserPass.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class UsersController(
	IUserManager userManager,
	ILSCoreAuthUserPassManager<string> lsCoreAuthorizeManager
) : ControllerBase
{
	[HttpPost]
	[Route("/login")]
	public string Login([FromBody] UserLoginRequest request) =>
		lsCoreAuthorizeManager.Authenticate(request.Username, request.Password).AccessToken;

	[HttpPut]
	[Route("/register")]
	public void Register([FromBody] UserRegisterRequest request) => userManager.Register(request);

	[HttpGet]
	[Route("/me")]
	public UserInformationDto Me() => userManager.Me();

	[HttpPost]
	[Route("/reset-password")]
	public void ResetPassword([FromBody] UserResetPasswordRequest request) =>
		userManager.ResetPassword(request);

	[HttpPut]
	[LSCoreAuth]
	[Route("/set-password")]
	public void SetPassword([FromBody] UserSetPasswordRequest request) =>
		userManager.SetPassword(request);
}
