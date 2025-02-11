using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Repository.EntityMaps;

namespace TD.Office.InterneOtpremnice.Repository;

public class InterneOtpremniceDbContext(DbContextOptions<InterneOtpremniceDbContext> options, IConfigurationRoot configurationRoot)
    : LSCoreDbContext<InterneOtpremniceDbContext>(options)
{
    public DbSet<InternaOtpremnicaEntity> InterneOtpremnice { get; set; }
    public DbSet<InternaOtpremnicaItemEntity> InterneOtpremniceItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(
            $"Server={configurationRoot["POSTGRES_HOST"]};Port={configurationRoot["POSTGRES_PORT"]};Userid={configurationRoot["POSTGRES_USER"]};Password={configurationRoot["POSTGRES_PASSWORD"]};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={configurationRoot["DEPLOY_ENV"]}_tdoffice_interne_otpremnice;Include Error Detail=true;",
            (action) =>
            {
                action.MigrationsAssembly("TD.Office.InterneOtpremnice.DbMigrations");
            }
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<InternaOtpremnicaEntity>().AddMap(new InternaOtpremnicaEntityMap());
        modelBuilder.Entity<InternaOtpremnicaItemEntity>().AddMap(new InternaOtpremnicaItemEntityMap());
    }
}
