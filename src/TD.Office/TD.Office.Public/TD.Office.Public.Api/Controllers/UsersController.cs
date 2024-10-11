using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Requests.Users;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [Route("/users")]
        [Permissions(Permission.Access, Permission.KorisniciRead)]
        public IActionResult GetMultiple([FromQuery] UsersGetMultipleRequest request) =>
            Ok(userManager.GetMultiple(request));
        
        [HttpGet]
        [Authorize]
        [Route("/users/{Id}")]
        [Permissions(Permission.Access, Permission.KorisniciRead)]
        public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
            Ok(userManager.GetSingle(request));
        
        /// <summary>
        /// Returns list of all permissions with their status for the specified user.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [Permissions(Permission.Access, Permission.KorisniciRead)]
        [Route("/users/{Id}/permissions")]
        public IActionResult GetPermissions([FromRoute] LSCoreIdRequest request) =>
            Ok(userManager.GetPermissions(request));
        
        [HttpPut]
        [Authorize]
        [Permissions(Permission.Access, Permission.KorisniciRead)]
        [Route("/users/{Id}/permissions/{Permission}")]
        public IActionResult UpdatePermission([FromRoute] LSCoreSaveRequest idRequest, [FromRoute] Permission Permission,
            [FromBody] UsersUpdatePermissionRequest request)
        {
            request.Id = idRequest.Id;
            request.Permission = Permission;
            
            userManager.UpdatePermission(request);
            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("/users/{Id}/nickname")]
        [Permissions(Permission.Access, Permission.KorisniciRead)]
        public IActionResult UpdateNickname([FromRoute] LSCoreIdRequest idRequest,
            [FromBody] UsersUpdateNicknameRequest request)
        {
            userManager.UpdateNickname(request);
            return Ok();
        }
        
        [HttpPost]
        [Authorize]
        [Route("/users")]
        [Permissions(Permission.Access, Permission.KorisniciRead)]
        public IActionResult Create([FromBody] UsersCreateRequest request) =>
            Ok(userManager.Create(request));
        
        [HttpPut]
        [Authorize]
        [Route("/users/{Id}/password")]
        [Permissions(Permission.Access, Permission.KorisniciRead)]
        public IActionResult UpdatePassword([FromRoute] LSCoreIdRequest idRequest,
            [FromBody] UsersUpdatePasswordRequest request)
        {
            request.Id = idRequest.Id;
            userManager.UpdatePassword(request);
            return Ok();
        }
    }
}
