using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Middlewares;

public class LastSeenMiddleware(RequestDelegate next, IUserManager userManager)
{
	public async Task Invoke(HttpContext context)
	{
		if (context.User.Identity is { IsAuthenticated: true })
			userManager.MarkLastSeen();

		await next(context);
	}
}
