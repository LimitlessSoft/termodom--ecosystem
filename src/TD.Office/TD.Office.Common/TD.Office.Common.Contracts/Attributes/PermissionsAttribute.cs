using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Attributes;

public class PermissionsAttribute (params Permission[] permissions) : Attribute
{
    public List<Permission> Permissions { get; } = permissions.ToList();
}