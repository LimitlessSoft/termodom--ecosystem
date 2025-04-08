using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TD.Office.MassSMS.Contracts.Constants;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Repository.EntityMaps;

namespace TD.Office.MassSMS.Repository;

public class MassSMSContext(
	DbContextOptions<MassSMSContext> options,
	IConfigurationRoot configurationRoot
) : LSCoreDbContext<MassSMSContext>(options)
{
	public DbSet<SMSEntity> SMSs { get; set; }
	public DbSet<SettingEntity> Settings { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		optionsBuilder.UseNpgsql(
			DbConstants.ConnectionString(configurationRoot),
			(action) =>
			{
				action.MigrationsAssembly(DbConstants.MigrationAssemblyName);
			}
		);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<SettingEntity>().AddMap(new SettingEntityMap());
		modelBuilder.Entity<SMSEntity>().AddMap(new SMSEntityMap());
	}
}
