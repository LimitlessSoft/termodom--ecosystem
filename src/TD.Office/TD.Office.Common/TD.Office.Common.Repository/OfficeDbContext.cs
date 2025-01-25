using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository.EntityMappings;

namespace TD.Office.Common.Repository
{
    public class OfficeDbContext(
        DbContextOptions<OfficeDbContext> options,
        IConfigurationRoot configurationRoot
    ) : LSCoreDbContext<OfficeDbContext>(options)
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
        public DbSet<NoteEntity> Notes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            optionsBuilder.UseNpgsql(
                $"Server={configurationRoot["POSTGRES_HOST"]};Port={configurationRoot["POSTGRES_PORT"]};Userid={configurationRoot["POSTGRES_USER"]};Password={configurationRoot["POSTGRES_PASSWORD"]};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={configurationRoot["DEPLOY_ENV"]}_tdoffice;Include Error Detail=true;",
                (action) =>
                {
                    action.MigrationsHistoryTable("migrations_history");
                    action.MigrationsAssembly("TD.Office.Common.DbMigrations");
                }
            );
        }

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
            modelBuilder.Entity<NoteEntity>().AddMap(new NotesEntityMap());
        }
    }
}
