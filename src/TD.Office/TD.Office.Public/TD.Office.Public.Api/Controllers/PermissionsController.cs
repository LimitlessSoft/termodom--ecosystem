using LSCore.Auth.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Contracts.Helpers;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Permissions;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[Permissions(Permission.Access)]
public class PermissionsController(
	LSCoreAuthContextEntity<string> contextEntity,
	OfficeDbContext dbContext
) : ControllerBase
{
	/// <summary>
	/// Returns current user's permissions for the specified permission group.
	/// </summary>
	/// <param name="permissionGroup"></param>
	/// <returns></returns>
	/// <exception cref="LSCoreForbiddenException"></exception>
	[HttpGet]
	[LSCoreAuth]
	[Route("/permissions/{permissionGroup}")]
	public IActionResult GetPermissions(string permissionGroup)
	{
		if (contextEntity.IsAuthenticated == false)
			throw new LSCoreForbiddenException();

		var relativePermissionsIds = Enum.GetValues<Permission>()
			.Where(p => p.HasPermissionGroupAttribute(permissionGroup));

		var user = dbContext
			.Users.AsNoTracking()
			.Include(u => u.Permissions)
			.First(u => u.Username == contextEntity.Identifier);
		
		

		return Ok(
			relativePermissionsIds.Select(p =>
			{
				var permission = user.Permissions?.FirstOrDefault(up =>
					up.Permission == p && up.IsActive
				);
				return new PermissionDto
				{
					Name = p.ToString(),
					Description = p.GetDescription()!,
					IsGranted = permission != null,
					Id = (long)p
				};
			})
		);
	}
}
