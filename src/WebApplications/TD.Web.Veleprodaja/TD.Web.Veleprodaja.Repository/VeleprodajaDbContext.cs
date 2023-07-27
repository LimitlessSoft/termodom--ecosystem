using Microsoft.EntityFrameworkCore;
using TD.Core.Repository;
using TD.Web.Veleprodaja.Contracts.Entities;
using TD.Web.Veleprodaja.Repository.DbMappings;

namespace TD.Web.Veleprodaja.Repository
{
    public class VeleprodajaDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrdersItems { get; set; }

        public VeleprodajaDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().AddMap(new UserMap());
            modelBuilder.Entity<Product>().AddMap(new ProductMap());
            modelBuilder.Entity<Order>().AddMap(new OrderMap());
            modelBuilder.Entity<OrderItem>().AddMap(new OrderItemMap());
        }
    }
}
