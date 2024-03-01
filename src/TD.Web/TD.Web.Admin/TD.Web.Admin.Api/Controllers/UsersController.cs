using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Users;
using TD.Web.Common.Contracts.Dtos.Users;
using LSCore.Contracts.Responses;

namespace TD.Web.Common.Api.Controllers
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

        [HttpGet]
        [Route("/user-product-price-levels")]
        public LSCoreListResponse<UserProductPriceLevelsDto> GetUserProductPriceGroupLevels([FromQuery] GetUserProductPriceLevelsRequest request) =>
            _userManager.GetUserProductPriceLevels(request);
    }
}
