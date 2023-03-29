using Infrastructure.Entities.ApiV2;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class ApiDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public ApiDbContext(DbContextOptions options) : base(options)
        {

        }

        public void OnConfiguring(DbContextOptions options)
        {

        }
    }
}
