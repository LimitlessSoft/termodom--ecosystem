using TD.Office.Common.Contracts.Attributes;

namespace TD.Office.Common.Contracts.Helpers;

public static class EnumsHelpers
{
    public static bool HasPermissionGroupAttribute<T>(this T enumValue, string permissionGroup)
    {
        var memberInfo = typeof(T).GetMember(enumValue!.ToString()!);
        var permissionGroupAttributes = memberInfo[0]
            .GetCustomAttributes(typeof(PermissionGroupAttribute), false)
            .ToList();
        return permissionGroupAttributes.Any(x =>
            (x as PermissionGroupAttribute)?.Name == permissionGroup
        );
    }
}
