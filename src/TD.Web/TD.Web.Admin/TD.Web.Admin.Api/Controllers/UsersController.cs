using LSCore.Auth.Contracts;
using LSCore.Auth.UserPass.Contracts;
using LSCore.Exceptions;
using LSCore.SortAndPage.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class UsersController(
	IUserManager userManager,
	ILSCoreAuthUserPassManager<string> lsCoreAuthorizeManager
) : ControllerBase
{
	[HttpPost]
	[AllowAnonymous]
	[Route("/login")]
	public string Login([FromBody] UserLoginRequest request) =>
		lsCoreAuthorizeManager.Authenticate(request.Username, request.Password).AccessToken;

	[HttpGet]
	[AllowAnonymous]
	[Route("/me")]
	public UserInformationDto Me() => userManager.Me();

	[HttpGet]
	[LSCoreAuth]
	[Route("/users")]
	[Permissions(Permission.Access)]
	public LSCoreSortedAndPagedResponse<UsersGetDto> GetUsers(
		[FromQuery] UsersGetRequest request
	) => userManager.GetUsers(request);

	[HttpGet]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users/{Username}")]
	public GetSingleUserDto GetSingleUser([FromRoute] GetSingleUserRequest request) =>
		userManager.GetSingleUser(request);

	[HttpPut]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users/{Username}/approve")]
	public void PutApproveUser([FromRoute] ApproveUserRequest request) =>
		userManager.ApproveUser(request);

	[HttpPut]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users/{Username}/type/{Type}")]
	public void PutUserType([FromRoute] PutUserTypeRequest request) =>
		userManager.PutUserType(request);

	[HttpPut]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users/{Username}/status/{IsActive}")]
	public void PutUserStatus([FromRoute] PutUserStatusRequest request) =>
		userManager.PutUserStatus(request);

	[HttpPut]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users/{Username}/get-ownership")]
	public void PutGetOwnership([FromRoute] GetOwnershipRequest request) =>
		userManager.GetOwnership(request);

	[HttpPut]
	[LSCoreAuth]
	[Route("/users")]
	[Permissions(Permission.Access)]
	public void UpdateUser([FromBody] UpdateUserRequest request) => userManager.UpdateUser(request);

	[HttpGet]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users-product-price-levels")]
	public List<UserProductPriceLevelsDto> GetUserProductPriceGroupLevels(
		[FromQuery] GetUserProductPriceLevelsRequest request
	) => userManager.GetUserProductPriceLevels(request);

	[HttpPut]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users-product-price-levels")]
	public void PutUserProductPriceGroupLevel([FromBody] PutUserProductPriceLevelRequest request) =>
		userManager.PutUserProductPriceLevel(request);

	[HttpPut]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users/{Username}/password")]
	public void ChangeUserPassword(
		[FromRoute] string Username,
		[FromBody] ChangeUserPasswordRequest request
	)
	{
		if (!Username.Equals(request.Username))
			throw new LSCoreBadRequestException();

		userManager.ChangeUserPassword(request);
	}

	[HttpGet]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users/phone-numbers")]
	public IActionResult GetPhoneNumbers() => Ok(userManager.GetPhoneNumbers());

	[HttpPost]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users-send-sms")]
	public async Task SendBulkSms([FromBody] SendBulkSmsRequest request) =>
		await userManager.SendBulkSms(request);

	[HttpGet]
	[LSCoreAuth]
	[Permissions(Permission.Access)]
	[Route("/users-analyze-ordered-products/{Username}")]
	public UsersAnalyzeOrderedProductsDto AnalyzeOrderedProducts(
		[FromRoute] string Username,
		[FromQuery] UsersAnalyzeOrderedProductsRequest request
	)
	{
		request.Username = Username;
		return userManager.AnalyzeOrderedProducts(request);
	}

	[HttpGet]
	[LSCoreAuth]
	[Route("/users-managing-products-groups/{Username}")]
	[Permissions(Permission.Access, Permission.Admin_Products_Access)]
	public List<long> GetManagingProductsGroups([FromRoute] string Username) =>
		userManager.GetManagingProductsGroups(Username);

	[HttpPost]
	[LSCoreAuth]
	[Route("/users-managing-products-groups/{Username}")]
	[Permissions(Permission.Access, Permission.Admin_Products_Access)]
	public void PostManagingProductsGroups(
		[FromRoute] string Username,
		[FromBody] List<long> managingGroups
	) => userManager.SetManagingProductsGroups(Username, managingGroups);

	[HttpGet]
	[LSCoreAuth]
	[Route("/users-managing-products/{Username}")]
	[Permissions(Permission.Access, Permission.Admin_Products_Access)]
	public List<long> GetManagingProducts([FromRoute] string Username) =>
		userManager.GetManagingProducts(Username);

	[HttpPost]
	[LSCoreAuth]
	[Route("/users-managing-products/{Username}")]
	[Permissions(Permission.Access, Permission.Admin_Products_Access)]
	public void PostManagingProducts(
		[FromRoute] string Username,
		[FromBody] List<long> productIds
	) => userManager.SetManagingProducts(Username, productIds);

	[HttpGet]
	[LSCoreAuth]
	[Route("/users/{Username}/permissions")]
	[Permissions(Permission.Access, Permission.Admin_Users_Access)]
	public IActionResult GetUserPermissions([FromRoute] string Username) =>
		Ok(userManager.GetUserPermissions(Username));

	[HttpPut]
	[LSCoreAuth]
	[Route("/users/{Username}/permissions")]
	[Permissions(Permission.Access, Permission.Admin_Users_Access, Permission.Admin_Users_Write)]
	public IActionResult SetUserPermissions(
		[FromRoute] string Username,
		[FromBody] List<int> permissionIds
	)
	{
		var permissions = permissionIds.Select(id => (Permission)id).ToList();
		userManager.SetUserPermissions(Username, permissions);
		return Ok();
	}
}
