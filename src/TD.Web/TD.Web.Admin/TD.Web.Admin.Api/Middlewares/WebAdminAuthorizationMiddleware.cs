using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Configurations;

namespace TD.Web.Admin.Api.Middlewares;

public class WebAdminAuthorizationMiddleware(
	RequestDelegate next,
	ApiKeysConfiguration apiKeysSettings
)
{
	public async Task Invoke(HttpContext context)
	{
		if (IsApiKeyAuthorized())
		{
			await next(context);
			return;
		}

		var endpoint = context.GetEndpoint();
		if (endpoint == null)
		{
			context.Response.StatusCode = StatusCodes.Status404NotFound;
			return;
		}

		var allowAnonymousAttribute = endpoint.Metadata.GetMetadata<IAllowAnonymous>();
		if (allowAnonymousAttribute != null)
		{
			await next(context);
			return;
		}

		if (!context.User.Identity?.IsAuthenticated ?? true)
		{
			context.Response.StatusCode = StatusCodes.Status401Unauthorized;
			return;
		}

		if (
			!LegacyConstants.DefaultAdminRoles.Any(role =>
				context.User.HasClaim(ClaimTypes.Role, role)
			)
		)
		{
			context.Response.StatusCode = StatusCodes.Status403Forbidden;
			return;
		}

		await next(context);
		return;

		bool IsApiKeyAuthorized()
		{
			return false;
			// var requestApiKey = context
			// 	.Request.Headers[LSCoreContractsConstants.ApiKeyCustomHeader]
			// 	.FirstOrDefault();
			// return !string.IsNullOrWhiteSpace(requestApiKey)
			// 	&& apiKeysSettings.ApiKeys.Contains(requestApiKey);
		}
	}
}
