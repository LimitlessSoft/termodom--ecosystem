using Microsoft.AspNetCore.Authorization;

namespace TD.Core.Framework
{
    public class AuthorizationAttribute : AuthorizeAttribute
    {
        public AuthorizationAttribute() { }
        public AuthorizationAttribute(params object[] roles)
        {
            if (roles.Any(x => x.GetType().BaseType != typeof(Enum)))
                throw new ArrayTypeMismatchException($"Authorization objects must be of type {typeof(Enum)}");

            base.Roles = string.Join(',', roles.Select(x => x.ToString()));
        }
    }
}
