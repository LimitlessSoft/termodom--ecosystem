using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Dtos.Users;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UsersController(IHttpContextAccessor httpContextAccessor, IUserManager userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _userManager.SetContext(_httpContextAccessor.HttpContext!);
        }

        [HttpPost]
        [Route("/login")]
        public LSCoreResponse<string> Login([FromBody] UsersLoginRequest request) =>
            _userManager.Login(request);

        [HttpGet]
        [Route("/me")]
        public LSCoreResponse<UserMeDto> Me() =>
            _userManager.Me();
        
        [HttpGet]
        [LSCoreAuthorization(UserType.SuperAdministrator)]
        [Route("/users")]
        public LSCoreSortedPagedResponse<UserDto> GetMultiple([FromQuery] UsersGetMultipleRequest request) =>
            _userManager.GetMultiple(request);
        
        [HttpGet]
        [LSCoreAuthorization]
        [Route("/users/{Id}")]
        public LSCoreResponse<UserDto> GetSingle([FromRoute] LSCoreIdRequest request) =>
            _userManager.GetSingle(request);

        [HttpPut]
        [LSCoreAuthorization(UserType.SuperAdministrator)]
        [Route("/users/{Id}/nickname")]
        public LSCoreResponse UpdateNickname([FromRoute] LSCoreIdRequest idRequest,
            [FromBody] UsersUpdateNicknameRequest request)
        {
            if (!request.Id.HasValue || idRequest.IdsNotMatch(request.Id!.Value))
                return LSCoreResponse.BadRequest();
                
            return _userManager.UpdateNickname(request);
        }
        
        [HttpPost]
        [Route("/users")]
        [LSCoreAuthorization(UserType.SuperAdministrator)]
        public LSCoreResponse<UserDto> Create([FromBody] UsersCreateRequest request) =>
            _userManager.Create(request);
    }
}
