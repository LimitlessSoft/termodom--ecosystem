using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Attributes;

public class PermissionsAttribute(params Permission[] permissions) : Attribute
{
	public List<Permission> Permissions { get; } = permissions.ToList();
}
