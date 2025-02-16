using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Managers;
using LSCore.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class UsersController (IUserManager userManager, LSCoreAuthorizeManager lsCoreAuthorizeManager) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [Route("/login")]
    public string Login([FromBody] UserLoginRequest request) =>
        lsCoreAuthorizeManager.Authorize(request.Username, request.Password).AccessToken;

    [HttpGet]
    [AllowAnonymous]
    [Route("/me")]
    public UserInformationDto Me() =>
        userManager.Me();

    [HttpGet]
    [LSCoreAuthorize]
    [Route("/users")]
    [Permissions(Permission.Access)]
    public LSCoreSortedAndPagedResponse<UsersGetDto> GetUsers([FromQuery] UsersGetRequest request) =>
        userManager.GetUsers(request);

    [HttpGet]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users/{Username}")]
    public GetSingleUserDto GetSingleUser([FromRoute] GetSingleUserRequest request) =>
        userManager.GetSingleUser(request);
        
    [HttpPut]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users/{Username}/approve")]
    public void PutApproveUser([FromRoute] ApproveUserRequest request) =>
        userManager.ApproveUser(request);
        
    [HttpPut]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users/{Username}/type/{Type}")]
    public void PutUserType([FromRoute] PutUserTypeRequest request) =>
        userManager.PutUserType(request);
        
    [HttpPut]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users/{Username}/status/{IsActive}")]
    public void PutUserStatus([FromRoute] PutUserStatusRequest request) =>
        userManager.PutUserStatus(request);
        
    [HttpPut]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users/{Username}/get-ownership")]
    public void PutGetOwnership([FromRoute] GetOwnershipRequest request) =>
        userManager.GetOwnership(request);

    [HttpPut]
    [LSCoreAuthorize]
    [Route("/users")]
    [Permissions(Permission.Access)]
    public void UpdateUser([FromBody] UpdateUserRequest request) =>
        userManager.UpdateUser(request);

    [HttpGet]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users-product-price-levels")]
    public List<UserProductPriceLevelsDto> GetUserProductPriceGroupLevels([FromQuery] GetUserProductPriceLevelsRequest request) =>
        userManager.GetUserProductPriceLevels(request);
        
    [HttpPut]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users-product-price-levels")]
    public void PutUserProductPriceGroupLevel([FromBody] PutUserProductPriceLevelRequest request) =>
        userManager.PutUserProductPriceLevel(request);

    [HttpPut]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users/{Username}/password")]
    public void ChangeUserPassword([FromRoute] string Username, [FromBody] ChangeUserPasswordRequest request)
    {
        if (!Username.Equals(request.Username))
            throw new LSCoreBadRequestException();
        
        userManager.ChangeUserPassword(request);
    }
        
    [HttpPost]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users-send-sms")]
    public async Task SendBulkSms([FromBody] SendBulkSmsRequest request) =>
        await userManager.SendBulkSms(request);

    [HttpGet]
    [LSCoreAuthorize]
    [Permissions(Permission.Access)]
    [Route("/users-analyze-ordered-products/{Username}")]
    public UsersAnalyzeOrderedProductsDto AnalyzeOrderedProducts([FromRoute] string Username,
        [FromQuery] UsersAnalyzeOrderedProductsRequest request)
    {
        request.Username = Username;
        return userManager.AnalyzeOrderedProducts(request);
    }
    
    [HttpGet]
    [LSCoreAuthorize]
    [Route("/users-managing-products-groups/{Username}")]
    [Permissions(Permission.Access, Permission.Admin_Products_Access)]
    public List<long> GetManagingProductsGroups([FromRoute] string Username) =>
        userManager.GetManagingProductsGroups(Username);
    
    [HttpPost]
    [LSCoreAuthorize]
    [Route("/users-managing-products-groups/{Username}")]
    [Permissions(Permission.Access, Permission.Admin_Products_Access)]
    public void PostManagingProductsGroups([FromRoute] string Username, [FromBody] List<long> managingGroups) =>
        userManager.SetManagingProductsGroups(Username, managingGroups);
}