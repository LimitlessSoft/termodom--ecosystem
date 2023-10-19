namespace TD.Web.Contracts.Helpers.Roles
{
    public static class RolesHelper
    {
        public static string CombineRoles(params string[] roles)
        {
            return string.Join(",", roles);
        }
    }
}
