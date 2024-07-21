using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD.Web.Admin.Contracts.Dtos.Permissions;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Api.Controllers;

[Authorize]
[Permissions(Permission.Access)]
public class PermissionsController(LSCoreContextUser contextUser, WebDbContext dbContext) : ControllerBase
{
    /// <summary>
    /// Returns current user's permissions for the specified permission group.
    /// </summary>
    /// <param name="permissionGroup"></param>
    /// <returns></returns>
    /// <exception cref="LSCoreForbiddenException"></exception>
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
            return new PermissionDto {
                Name = p.ToString(),
                Description = p.GetDescription()!,
                IsGranted = permission != null,
                Id = (long)p
            };
        }));
    }
}