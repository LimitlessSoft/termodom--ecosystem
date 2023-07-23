using Microsoft.EntityFrameworkCore;
using TD.Web.Veleprodaja.Contracts.Entities;

namespace TD.Web.Veleprodaja.Repository
{
    public class VeleprodajaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public VeleprodajaDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
