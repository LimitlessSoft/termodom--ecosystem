using Microsoft.EntityFrameworkCore;

namespace TD.Web.Repository
{
    public class WebDbContext : DbContext
    {
        public WebDbContext(DbContextOptions otpions) : base(otpions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
