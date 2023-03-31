using Infrastructure.Entities.ApiV2;
using Microsoft.EntityFrameworkCore;

namespace DBMigrations
{
    public class DbMigrationsDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbMigrationsDbContext(DbContextOptions options) : base(options)
        {

        }

        public void OnConfiguring(DbContextOptions options)
        {

        }
    }
}
