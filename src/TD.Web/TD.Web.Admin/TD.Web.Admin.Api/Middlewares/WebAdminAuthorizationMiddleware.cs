using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using LSCore.Contracts;
using LSCore.Contracts.SettingsModels;
using TD.Web.Common.Contracts;

namespace TD.Web.Admin.Api.Middlewares
{
    public class WebAdminAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly LSCoreApiKeysSettings _apiKeySettings;

        public WebAdminAuthorizationMiddleware(RequestDelegate next, LSCoreApiKeysSettings apiKeysSettings)
        {
            _next = next;
            _apiKeySettings = apiKeysSettings;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsApiKeyAuthorized())
            {
                await _next(context);
                return;
            }
            
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
            return;

            bool IsApiKeyAuthorized()
            {
                var requestApiKey = context.Request.Headers[LSCoreContractsConstants.ApiKeyCustomHeader].FirstOrDefault();
                return !string.IsNullOrWhiteSpace(requestApiKey) && _apiKeySettings.ApiKeys.Contains(requestApiKey);
            }
        }
    }
}
