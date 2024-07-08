using TD.Office.Common.Contracts.Attributes;

namespace TD.Office.Common.Contracts.Helpers;

public static class EnumsHelpers
{
    public static bool HasPermissionGroupAttribute<T>(this T enumValue, string permissionGroup)
    {
        var memberInfo = typeof(T).GetMember(enumValue!.ToString()!);
        var permissionGroupAttribute = (PermissionGroupAttribute?)memberInfo[0].GetCustomAttributes(typeof(PermissionGroupAttribute), false).FirstOrDefault();
        return permissionGroupAttribute?.Name == permissionGroup;
    }
}