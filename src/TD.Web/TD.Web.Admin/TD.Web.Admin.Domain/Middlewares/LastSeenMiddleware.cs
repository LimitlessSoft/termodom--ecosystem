using Microsoft.AspNetCore.Http;
using TD.Web.Admin.Domain.Managers;

namespace TD.Web.Admin.Domain.Middlewares
{
    public class LastSeenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly UserManager _userManager;

        public LastSeenMiddleware(RequestDelegate next, UserManager userManager)
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