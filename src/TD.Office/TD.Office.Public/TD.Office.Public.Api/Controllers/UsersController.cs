using LSCore.Auth.Contracts;
using LSCore.Auth.UserPass.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.InterneOtpremnice.Contracts.Requests;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Api.Controllers;

[ApiController]
public class UsersController(
	IHttpContextAccessor httpContextAccessor,
	IUserManager userManager,
	ILSCoreAuthUserPassManager<string> authManager
) : ControllerBase
{
	[HttpPost]
	[Route("/login")]
	public IActionResult Login([FromBody] UsersLoginRequest request) =>
		Ok(authManager.Authenticate(request.Username!, request.Password!).AccessToken);

	[HttpGet]
	[Route("/me")]
	public IActionResult Me() => Ok(userManager.Me());

	[HttpGet]
	[LSCoreAuth]
	[Route("/users")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult GetMultiple([FromQuery] UsersGetMultipleRequest request) =>
		Ok(userManager.GetMultiple(request));

	[HttpGet]
	[LSCoreAuth]
	[Route("/users/{Id}")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
		Ok(userManager.GetSingle(request));

	/// <summary>
	/// Returns list of all permissions with their status for the specified user.
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpGet]
	[LSCoreAuth]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	[Route("/users/{Id}/permissions")]
	public IActionResult GetPermissions([FromRoute] LSCoreIdRequest request) =>
		Ok(userManager.GetPermissions(request));

	[HttpPut]
	[LSCoreAuth]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	[Route("/users/{Id}/permissions/{Permission}")]
	public IActionResult UpdatePermission(
		[FromRoute] IdRequest idRequest,
		[FromRoute] Permission Permission,
		[FromBody] UsersUpdatePermissionRequest request
	)
	{
		request.Id = idRequest.Id;
		request.Permission = Permission;

		userManager.UpdatePermission(request);
		return Ok();
	}

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Id}/nickname")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult UpdateNickname(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] UsersUpdateNicknameRequest request
	)
	{
		userManager.UpdateNickname(request);
		return Ok();
	}

	[HttpPost]
	[LSCoreAuth]
	[Route("/users")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult Create([FromBody] UsersCreateRequest request) =>
		Ok(userManager.Create(request));

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Id}/password")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult UpdatePassword(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] UsersUpdatePasswordRequest request
	)
	{
		request.Id = idRequest.Id;
		userManager.UpdatePassword(request);
		return Ok();
	}

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Id}/max-rabat-mp-dokumenti")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult UpdateMaxRabatMpDokumenti(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] UpdateMaxRabatMPDokumentiRequest request
	)
	{
		request.Id = idRequest.Id;
		userManager.UpdateMaxRabatMpDokumenti(request);
		return Ok();
	}

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Id}/max-rabat-vp-dokumenti")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult UpdateMaxRabatVpDokumenti(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] UpdateMaxRabatVPDokumentiRequest request
	)
	{
		request.Id = idRequest.Id;
		userManager.UpdateMaxRabatVpDokumenti(request);
		return Ok();
	}

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Id}/store-id")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult UpdateStoreId(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] UpdateStoreIdRequest request
	)
	{
		request.Id = idRequest.Id;
		userManager.UpdateStoreId(request);
		return Ok();
	}

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Id}/vp-magacin-id")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult UpdateStoreId(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] UpdateVPMagacinIdRequest request
	)
	{
		request.Id = idRequest.Id;
		userManager.UpdateVPMagacinId(request);
		return Ok();
	}

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Id}/tip-korisnika-id")]
	[Permissions(Permission.Access, Permission.KorisniciListRead)]
	public IActionResult UpdateTipKorisnikaId(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] UpdateTipKorisnikaIdRequest request
	)
	{
		request.Id = idRequest.Id;
		userManager.UpdateTipKorisnikaId(request);
		return Ok();
	}
}
