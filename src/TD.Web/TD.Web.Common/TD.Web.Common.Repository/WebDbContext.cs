using TD.Web.Common.Repository.DbMappings;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using LSCore.Repository;

namespace TD.Web.Common.Repository
{
    public class WebDbContext(DbContextOptions<WebDbContext> options) : LSCoreDbContext<WebDbContext>(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ProductGroupEntity> ProductGroups { get; set; }
        public DbSet<UnitEntity> Units { get; set; }
        public DbSet<ProductPriceEntity> ProductPrices { get; set; }
        public DbSet<ProductPriceGroupEntity> ProductPriceGroups { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<ProductPriceGroupLevelEntity> ProductPriceGroupLevel { get; set; }
        public DbSet<CityEntity> Cities { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
        public DbSet<KomercijalnoWebProductLinkEntity> KomercijalnoWebProductLinks { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }
        public DbSet<PaymentTypeEntity> PaymentTypes { get; set; }
        public DbSet<GlobalAlertEntity> GlobalAlerts { get; set; }
        public DbSet<OrderOneTimeInformationEntity> OrderOneTimeInformation { get; set; }
        public DbSet<StoreEntity> Stores { get; set; }
        public DbSet<ProfessionEntity> Professions { get; set; }
        public DbSet<StatisticsItemEntity> StatisticsItems { get; set; }

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
            modelBuilder.Entity<CityEntity>().AddMap(new CityEntityMap());
            modelBuilder.Entity<OrderItemEntity>().AddMap(new OrderItemEntityMap());
            modelBuilder.Entity<KomercijalnoWebProductLinkEntity>().AddMap(new KomercijalnoWebProductLinkEntityMap());
            modelBuilder.Entity<SettingEntity>().AddMap(new SettingEntityMap());
            modelBuilder.Entity<PaymentTypeEntity>().AddMap(new PaymentTypeEntityMap());
            modelBuilder.Entity<GlobalAlertEntity>().AddMap(new GlobalAlertEntityMap());
            modelBuilder.Entity<OrderOneTimeInformationEntity>().AddMap(new OrderOneTimeInformationEntityMap());
            modelBuilder.Entity<StoreEntity>().AddMap(new StoreEntityMap());
            modelBuilder.Entity<ProfessionEntity>().AddMap(new ProfessionEntityMap());
            modelBuilder.Entity<StatisticsItemEntity>().AddMap(new StatisticsItemEntityMap());
        }
    }
}
