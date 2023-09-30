using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TD.Web.Repository;

namespace TD.Web.Domain.Middlewares
{
    public class LastSeenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly WebDbContext _dbContext;

        public LastSeenMiddleware(RequestDelegate next, WebDbContext dbContext)
        {
            _next = next;
            _dbContext = dbContext;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User.Identity != null && context.User.Identity.IsAuthenticated)
            {
                var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var user = await _dbContext.Users.FindAsync(userId);

                if (user != null)
                {
                    user.LastTimeSeen = DateTime.UtcNow;
                    await _dbContext.SaveChangesAsync();
                }
            }

            await _next(context);
        }
    }
}