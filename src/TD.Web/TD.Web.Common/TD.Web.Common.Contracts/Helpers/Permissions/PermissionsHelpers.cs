using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces;

namespace TD.Web.Common.Contracts.Helpers.Permissions;

public static class PermissionsHelpers
{
	public static async Task<bool> HasRequiredPermissionsAsync(
		IWebDbContext dbContext,
		string username,
		List<Permission> requiredPermissions)
	{
		var user = await dbContext.Users
			.AsNoTracking()
			.FirstOrDefaultAsync(u => u.IsActive && u.Username == username);

		if (user == null)
			return false;

		// SuperAdmin has all permissions
		if (user.Type == UserType.SuperAdmin)
			return true;

		var userPermissions = await dbContext.UserPermissions
			.AsNoTracking()
			.Where(p => p.UserId == user.Id && p.IsActive)
			.Select(p => p.Permission)
			.ToListAsync();

		// User must have ALL required permissions
		return requiredPermissions.All(required => userPermissions.Contains(required));
	}
}
