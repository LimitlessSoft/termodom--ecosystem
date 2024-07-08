using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using Microsoft.AspNetCore.Authorization;
using TD.Office.Common.Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Helpers;
using TD.Office.Common.Repository;

namespace TD.Office.Public.Api.Controllers;

public class PermissionsController(LSCoreContextUser contextUser, OfficeDbContext dbContext) : ControllerBase
{
    [HttpGet]
    [Authorize]
    [Route("/permissions/{permissionGroup}")]
    public IActionResult GetPermissions(string permissionGroup)
    {
        if (contextUser.Id == null)
            throw new LSCoreForbiddenException();

        var relativePermissionsIds = Enum.GetValues<Permission>()
            .Where(p => p.HasPermissionGroupAttribute(permissionGroup));
        
        var user = dbContext.Users
            .AsNoTracking()
            .Include(u => u.Permissions)
            .First(u => u.Id == contextUser.Id);
        
        return Ok(relativePermissionsIds.Select(p => {
            var permission = user.Permissions?.FirstOrDefault(up => up.Permission == p && up.IsActive);
            return new {
                Permission = p.ToString(),
                Description = p.GetDescription(),
                IsGranted = permission != null
            };
        }));
    }
}