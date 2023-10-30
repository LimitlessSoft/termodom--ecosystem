using Microsoft.EntityFrameworkCore;
using TD.Core.Repository;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository.DbMappings;

namespace TD.Web.Common.Repository
{
    public class WebDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductGroupEntity> ProductGroups { get; set; }
        public DbSet<UnitEntity> Units { get; set; }
        public DbSet<ProductPriceEntity> ProductPrices { get; set; }
        public DbSet<ProductPriceGroupEntity> ProductPriceGroups { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductPriceGroupLevelEntity> ProductPriceGroupLevel { get; set; }

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
            modelBuilder.Entity<OrderEntity>().AddMap(new OrderEntityMap());
            modelBuilder.Entity<ProductPriceGroupLevelEntity>().AddMap(new ProductPriceGroupLevelEntityMap());
        }
    }
}
