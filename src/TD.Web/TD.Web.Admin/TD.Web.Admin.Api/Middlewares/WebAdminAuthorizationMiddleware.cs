using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TD.Web.Common.Contracts;

namespace TD.Web.Admin.Api.Middlewares
{
    public class WebAdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public WebAdminAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if(endpoint == null)
            {
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                return;
            }

            var allowAnonymousAttribute = endpoint.Metadata.GetMetadata<IAllowAnonymous>();
            if (allowAnonymousAttribute != null)
            {
                await _next(context);
                return;
            }

            if (!context.User.Identity?.IsAuthenticated ?? true)
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }

            if (!Constants.DefaultAdminRoles.Any(role => context.User.HasClaim(ClaimTypes.Role, role)))
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await _next(context);
        }
    }
}
