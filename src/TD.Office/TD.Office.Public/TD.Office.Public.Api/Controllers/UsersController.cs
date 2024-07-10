using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Requests.Users;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;

namespace TD.Office.Public.Api.Controllers
{
    [ApiController]
    public class UsersController (IHttpContextAccessor httpContextAccessor, IUserManager userManager)
        : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        [HttpPost]
        [Route("/login")]
        public IActionResult Login([FromBody] UsersLoginRequest request) =>
            Ok(userManager.Login(request));

        [HttpGet]
        [Route("/me")]
        public IActionResult Me() =>
            Ok(userManager.Me());
        
        [HttpGet]
        [Route("/users")]
        public IActionResult GetMultiple([FromQuery] UsersGetMultipleRequest request) =>
            Ok(userManager.GetMultiple(request));
        
        [HttpGet]
        [Route("/users/{Id}")]
        public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
            Ok(userManager.GetSingle(request));
        
        /// <summary>
        /// Returns list of all permissions with their status for the specified user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/users/{Id}/permissions")]
        public IActionResult GetPermissions([FromRoute] LSCoreIdRequest request) =>
            Ok(userManager.GetPermissions(request));

        [HttpPut]
        [Route("/users/{Id}/nickname")]
        public IActionResult UpdateNickname([FromRoute] LSCoreIdRequest idRequest,
            [FromBody] UsersUpdateNicknameRequest request)
        {
            userManager.UpdateNickname(request);
            return Ok();
        }
        
        [HttpPost]
        [Route("/users")]
        public IActionResult Create([FromBody] UsersCreateRequest request) =>
            Ok(userManager.Create(request));
    }
}
