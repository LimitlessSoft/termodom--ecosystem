using Microsoft.EntityFrameworkCore;
using TD.Webshop.Contracts.Entities;

namespace TD.Webshop.Repository
{
    public class WebshopDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public WebshopDbContext(DbContextOptions options) : base(options)
        {

        }
    }
}
