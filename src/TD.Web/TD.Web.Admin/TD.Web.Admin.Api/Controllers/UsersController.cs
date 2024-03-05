using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;

namespace TD.Web.Admin.Api.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        public UsersController(IUserManager userManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _userManager.SetContext(httpContextAccessor.HttpContext!);
        }

        [HttpPost]
        [Route("/login")]
        public LSCoreResponse<string> Login([FromBody] UserLoginRequest request) =>
            _userManager.Login(request);

        [HttpGet]
        [Route("/me")]
        public LSCoreResponse<UserInformationDto> Me() =>
            _userManager.Me();

        [HttpGet]
        [Route("/users")]
        public LSCoreSortedPagedResponse<UsersGetDto> GetUsers([FromQuery] UsersGetRequest request) =>
            _userManager.GetUsers(request);

        [HttpGet]
        [Route("/users/{Username}")]
        public LSCoreResponse<GetSingleUserDto> GetSingleUser([FromRoute] GetSingleUserRequest request) =>
            _userManager.GetSingleUser(request);
        
        [HttpPut]
        [Route("/users/{Username}/approve")]
        public LSCoreResponse PutApproveUser([FromRoute] ApproveUserRequest request) =>
            _userManager.ApproveUser(request);
        
        [HttpPut]
        [Route("/users/{Username}/type/{Type}")]
        public LSCoreResponse PutUserType([FromRoute] PutUserTypeRequest request) =>
            _userManager.PutUserType(request);
        
        [HttpPut]
        [Route("/users/{Username}/status/{Status}")]
        public LSCoreResponse PutUserStatus([FromRoute] PutUserStatusRequest request) =>
            _userManager.PutUserStatus(request);
        
        [HttpPut]
        [Route("/users/{Username}/get-ownership")]
        public LSCoreResponse PutGetOwnership([FromRoute] GetOwnershipRequest request) =>
            _userManager.GetOwnership(request);

        [HttpPut]
        [Route("/users")]
        public LSCoreResponse UpdateUser([FromBody] UpdateUserRequest request) =>
            _userManager.UpdateUser(request);

        [HttpGet]
        [Route("/users-product-price-levels")]
        public LSCoreListResponse<UserProductPriceLevelsDto> GetUserProductPriceGroupLevels([FromQuery] GetUserProductPriceLevelsRequest request) =>
            _userManager.GetUserProductPriceLevels(request);
        
        [HttpPut]
        [Route("/users-product-price-levels")]
        public LSCoreResponse PutUserProductPriceGroupLevel([FromBody] PutUserProductPriceLevelRequest request) =>
            _userManager.PutUserProductPriceLevel(request);
    }
}
