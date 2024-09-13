using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository.EntityMappings;

namespace TD.Office.Common.Repository
{
    public class OfficeDbContext(DbContextOptions<OfficeDbContext> options)
        : LSCoreDbContext<OfficeDbContext>(options)
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<KomercijalnoPriceEntity> KomercijalnoPrices { get; set; }
        public DbSet<UslovFormiranjaWebCeneEntity> UsloviFormiranjaWebcena { get; set; }
        public DbSet<NalogZaPrevozEntity> NaloziZaPrevoz { get; set; }
        public DbSet<UserPermissionEntity> UserPermissions { get; set; }
        public DbSet<SettingEntity> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserEntity>().AddMap(new UserEntityMap());
            modelBuilder.Entity<KomercijalnoPriceEntity>().AddMap(new KomercijalnoPriceEntityMap());
            modelBuilder
                .Entity<UslovFormiranjaWebCeneEntity>()
                .AddMap(new UslovFormiranjaWebCeneEntityMap());
            modelBuilder.Entity<NalogZaPrevozEntity>().AddMap(new NalogZaPrevozEntityMap());
            modelBuilder.Entity<UserPermissionEntity>().AddMap(new UserPermissionEntityMap());
            modelBuilder.Entity<SettingEntity>().AddMap(new SettingEntityMap());
        }
    }
}
