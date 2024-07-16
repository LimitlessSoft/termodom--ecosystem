using TD.Office.Public.Contracts.Dtos.Permissions;
using TD.Office.Common.Contracts.Attributes;
using Microsoft.AspNetCore.Authorization;
using TD.Office.Common.Contracts.Helpers;
using TD.Office.Common.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Repository;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Exceptions;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts;

namespace TD.Office.Public.Api.Controllers;

[Authorize]
[Permissions(Permission.Access)]
public class PermissionsController(LSCoreContextUser contextUser, OfficeDbContext dbContext) : ControllerBase
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