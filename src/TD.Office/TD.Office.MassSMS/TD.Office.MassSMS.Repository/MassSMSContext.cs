using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TD.Office.MassSMS.Repository;

public class MassSMSContext(
	DbContextOptions<MassSMSContext> options,
	IConfigurationRoot configurationRoot
) : LSCoreDbContext<MassSMSContext>(options)
{
	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.UseNpgsql(
			$"Server={configurationRoot["POSTGRES_HOST"]};Port={configurationRoot["POSTGRES_PORT"]};Userid={configurationRoot["POSTGRES_USER"]};Password={configurationRoot["POSTGRES_PASSWORD"]};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={configurationRoot["DEPLOY_ENV"]}_tdoffice_interne_otpremnice;Include Error Detail=true;",
			(action) =>
			{
				action.MigrationsAssembly("TD.Office.MassSMS.DbMigrations");
			}
		);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
	}
}
