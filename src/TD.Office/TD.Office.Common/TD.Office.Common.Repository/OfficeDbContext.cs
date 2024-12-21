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
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<SpecifikacijaNovcaEntity> SpecifikacijeNovca { get; set; }
        public DbSet<ProracunEntity> Proracuni { get; set; }
        public DbSet<ProracunItemEntity> ProracunItems { get; set; }
        public DbSet<KomercijalnoIFinansijskoPoGodinamaStatusEntity> KomercijalnoIFinansijskoPoGodinamaStatus { get; set; }
        public DbSet<KomercijalnoIFinansijskoPoGodinamaEntity> KomercijalnoIFinansijskoPoGodinama { get; set; }
        public DbSet<MagacinCentarEntity> MagacinCentri { get; set; }

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
            modelBuilder.Entity<LogEntity>().AddMap(new LogEntityMap());
            modelBuilder
                .Entity<SpecifikacijaNovcaEntity>()
                .AddMap(new SpecifikacijaNovcaEntityMap());
            modelBuilder.Entity<ProracunEntity>().AddMap(new ProracunEntityMap());
            modelBuilder.Entity<ProracunItemEntity>().AddMap(new ProracunItemEntityMap());
            modelBuilder
                .Entity<KomercijalnoIFinansijskoPoGodinamaStatusEntity>()
                .AddMap(new KomercijalnoIFinansijskoPoGodinamaStatusEntityMap());
            modelBuilder
                .Entity<KomercijalnoIFinansijskoPoGodinamaEntity>()
                .AddMap(new KomercijalnoIFinansijskoPoGodinamaEntityMap());
            modelBuilder.Entity<MagacinCentarEntity>().AddMap(new MagacinCentarEntityMap());
        }
    }
}
