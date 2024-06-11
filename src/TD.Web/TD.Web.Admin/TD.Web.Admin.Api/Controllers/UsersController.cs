using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Admin.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using Microsoft.AspNetCore.Authorization;
using LSCore.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class UsersController (IUserManager userManager) : ControllerBase
{
    [HttpPost]
    [AllowAnonymous]
    [Route("/login")]
    public string Login([FromBody] UserLoginRequest request) =>
        userManager.Login(request);

    [HttpGet]
    [AllowAnonymous]
    [Route("/me")]
    public UserInformationDto Me() =>
        userManager.Me();

    [HttpGet]
    [Route("/users")]
    public List<UsersGetDto> GetUsers([FromQuery] UsersGetRequest request) =>
        userManager.GetUsers(request);

    [HttpGet]
    [Route("/users/{Username}")]
    public GetSingleUserDto GetSingleUser([FromRoute] GetSingleUserRequest request) =>
        userManager.GetSingleUser(request);
        
    [HttpPut]
    [Route("/users/{Username}/approve")]
    public void PutApproveUser([FromRoute] ApproveUserRequest request) =>
        userManager.ApproveUser(request);
        
    [HttpPut]
    [Route("/users/{Username}/type/{Type}")]
    public void PutUserType([FromRoute] PutUserTypeRequest request) =>
        userManager.PutUserType(request);
        
    [HttpPut]
    [Route("/users/{Username}/status/{IsActive}")]
    public void PutUserStatus([FromRoute] PutUserStatusRequest request) =>
        userManager.PutUserStatus(request);
        
    [HttpPut]
    [Route("/users/{Username}/get-ownership")]
    public void PutGetOwnership([FromRoute] GetOwnershipRequest request) =>
        userManager.GetOwnership(request);

    [HttpPut]
    [Route("/users")]
    public void UpdateUser([FromBody] UpdateUserRequest request) =>
        userManager.UpdateUser(request);

    [HttpGet]
    [Route("/users-product-price-levels")]
    public List<UserProductPriceLevelsDto> GetUserProductPriceGroupLevels([FromQuery] GetUserProductPriceLevelsRequest request) =>
        userManager.GetUserProductPriceLevels(request);
        
    [HttpPut]
    [Route("/users-product-price-levels")]
    public void PutUserProductPriceGroupLevel([FromBody] PutUserProductPriceLevelRequest request) =>
        userManager.PutUserProductPriceLevel(request);

    [HttpPut]
    [Route("/users/{Username}/password")]
    public void ChangeUserPassword([FromRoute] string Username, [FromBody] ChangeUserPasswordRequest request)
    {
        if (!Username.Equals(request.Username))
            throw new LSCoreBadRequestException();
        
        userManager.ChangeUserPassword(request);
    }
        
    [HttpPost]
    [Route("/users-send-sms")]
    public async Task  SendBulkSms([FromBody] SendBulkSmsRequest request) =>
        await userManager.SendBulkSms(request);

    [HttpGet]
    [Route("/users-analyze-ordered-products/{Username}")]
    public UsersAnalyzeOrderedProductsDto AnalyzeOrderedProducts([FromRoute] string Username,
        [FromQuery] UsersAnalyzeOrderedProductsRequest request)
    {
        request.Username = Username;
        return userManager.AnalyzeOrderedProducts(request);
    }
}