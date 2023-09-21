using Microsoft.EntityFrameworkCore;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;
using TD.Web.Repository.DbMappings;

namespace TD.Web.Repository
{
    public class WebDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductGroupEntity> ProductGroups { get; set; }
        public DbSet<UnitsEntity> Units { get; set; }

        public WebDbContext(DbContextOptions otpions) : base(otpions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().AddMap(new UserEntityMap());
            modelBuilder.Entity<ProductEntity>().AddMap(new ProductEntityMap());
            modelBuilder.Entity<ProductGroupEntity>().AddMap(new ProductGroupEntityMap());
            modelBuilder.Entity<UnitsEntity>().AddMap(new UnitsEntityMap());
        }
    }
}
