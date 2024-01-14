using LSCore.Contracts.IManagers;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository.EntityMappings;

namespace TD.Office.Common.Repository
{
    public class OfficeDbContext : DbContext, ILSCoreDbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<KomercijalnoPriceEntity> KomercijalnoPrices { get; set; }
        public DbSet<UslovFormiranjaWebCeneEntity> UsloviFormiranjaWebcena { get; set; }

        public OfficeDbContext(DbContextOptions otpions) : base(otpions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().AddMap(new UserEntityMap());
            modelBuilder.Entity<KomercijalnoPriceEntity>().AddMap(new KomercijalnoPriceEntityMap());
            modelBuilder.Entity<UslovFormiranjaWebCeneEntity>().AddMap(new UslovFormiranjaWebCeneEntityMap());
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
