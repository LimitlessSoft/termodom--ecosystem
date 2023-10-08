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
        public DbSet<UnitEntity> Units { get; set; }
        public DbSet<ProductPriceEntity> ProductPrices { get; set; }
        public DbSet<ProductPriceGroupEntity> ProductPriceGroupEntities { get; set; }

        public WebDbContext(DbContextOptions otpions) : base(otpions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().AddMap(new UserEntityMap());
            modelBuilder.Entity<ProductEntity>().AddMap(new ProductEntityMap());
            modelBuilder.Entity<ProductGroupEntity>().AddMap(new ProductGroupEntityMap());
            modelBuilder.Entity<UnitEntity>().AddMap(new UnitEntityMap());
            modelBuilder.Entity<ProductPriceEntity>().AddMap(new ProductPriceEntityMap());
            modelBuilder.Entity<ProductPriceGroupEntity>().AddMap(new ProductPriceGroupEntityMap());
        }
    }
}
