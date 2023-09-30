using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;
using TD.Web.Domain.Managers;
using TD.Web.Repository;

namespace TD.Web.Domain.Middlewares
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
            _userManager.SetContextInfo(context);

            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
                _userManager.MarkLastSeen();

            await _next(context);
        }
    }
}