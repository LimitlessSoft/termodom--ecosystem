using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository.EntityMappings;

namespace MigracijaSpecifikacijaNovca;

public class MigrationDbContext : DbContext
{
	private readonly string _connectionString;

	public DbSet<SpecifikacijaNovcaEntity> SpecifikacijeNovca { get; set; }

	public MigrationDbContext(string connectionString)
	{
		_connectionString = connectionString;
	}

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		optionsBuilder.UseNpgsql(_connectionString);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<SpecifikacijaNovcaEntity>(builder =>
		{
			builder.ToTable("SpecifikacijaNovca");
			builder.HasKey(x => x.Id);
		});
	}
}
