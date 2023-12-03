using Microsoft.AspNetCore.Http;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Domain.Middlewares
{
    public class LastSeenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IUserManager _userManager;

        public LastSeenMiddleware(RequestDelegate next, IUserManager userManager)
        {
            _next = next;
            _userManager = userManager;
        }

        public async Task Invoke(HttpContext context)
        {
            _userManager.SetContext(context);

            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
                _userManager.MarkLastSeen();

            await _next(context);
        }
    }
}