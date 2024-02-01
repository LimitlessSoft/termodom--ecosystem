using LSCore.Contracts.IManagers;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository.DbMappings;

namespace TD.Web.Common.Repository
{
    public class WebDbContext : DbContext, ILSCoreDbContext
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
            modelBuilder.Entity<CityEntity>().AddMap(new CityEntityMap());
            modelBuilder.Entity<OrderItemEntity>().AddMap(new OrderItemEntityMap());
            modelBuilder.Entity<KomercijalnoWebProductLinkEntity>().AddMap(new KomercijalnoWebProductLinkEntityMap());
            modelBuilder.Entity<SettingEntity>().AddMap(new SettingEntityMap());
            modelBuilder.Entity<PaymentTypeEntity>().AddMap(new PaymentTypeEntityMap());
            modelBuilder.Entity<GlobalAlertEntity>().AddMap(new GlobalAlertEntityMap());
            modelBuilder.Entity<OrderOneTimeInformationEntity>().AddMap(new OrderOneTimeInformationEntityMap());
            modelBuilder.Entity<StoreEntity>().AddMap(new StoreEntityMap());
        }

        public IQueryable<T> AsQueryable<T>() where T : class =>
            base.Set<T>().AsQueryable();

        public List<T> SqlQuery<T>(string query) where T : class, new() =>
            base.Set<T>().FromSqlRaw(query).ToList();

        public void Insert<T>(T entity) where T : class
        {
            base.Set<T>().Add(entity);
            base.SaveChanges();
        }

        public void InsertMultiple<T>(IEnumerable<T> entities) where T : class
        {
            base.Set<T>().AddRange(entities);
            base.SaveChanges();
        }

        void ILSCoreDbContext.Update<T>(T entity) =>
            base.Set<T>().Update(entity);

        public void Delete<T>(T entity) where T : class
        {
            base.Set<T>().Remove(entity);
            base.SaveChanges();
        }

        public void Delete<T>(IEnumerable<T> entities) where T : class
        {
            base.Set<T>().RemoveRange(entities);
            base.SaveChanges();
        }

        public void DeleteNonEntity<T>(Expression<Func<T, bool>> expression) where T : class
        {
            base.Set<T>().RemoveRange(base.Set<T>().Where(expression));
            base.SaveChanges();
        }

        public T Get<T>(Expression<Func<T, bool>> expression) where T : class =>
            base.Set<T>().FirstOrDefault(expression);

        public void UpdateMultiple<T>(IEnumerable<T> entities) where T : class
        {
            base.Set<T>().UpdateRange(entities);
            base.SaveChanges();
        }
    }
}
