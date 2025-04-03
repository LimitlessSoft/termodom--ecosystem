using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Npgsql;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces;
using TD.Web.Common.Repository.DbMappings;

namespace TD.Web.Common.Repository;

public class WebDbContext(
	DbContextOptions<WebDbContext> options,
	IConfigurationRoot configurationRoot
) : LSCoreDbContext<WebDbContext>(options), IWebDbContext
{
	public DbSet<UnitEntity> Units { get; set; }
	public DbSet<UserEntity> Users { get; set; }
	public DbSet<CityEntity> Cities { get; set; }
	public DbSet<OrderEntity> Orders { get; set; }
	public DbSet<StoreEntity> Stores { get; set; }
	public DbSet<ProductEntity> Products { get; set; }
	public DbSet<SettingEntity> Settings { get; set; }
	public DbSet<OrderItemEntity> OrderItems { get; set; }
	public DbSet<ProfessionEntity> Professions { get; set; }
	public DbSet<GlobalAlertEntity> GlobalAlerts { get; set; }
	public DbSet<PaymentTypeEntity> PaymentTypes { get; set; }
	public DbSet<ProductGroupEntity> ProductGroups { get; set; }
	public DbSet<ProductPriceEntity> ProductPrices { get; set; }
	public DbSet<StatisticsItemEntity> StatisticsItems { get; set; }
	public DbSet<ProductPriceGroupEntity> ProductPriceGroups { get; set; }
	public DbSet<ProductPriceGroupLevelEntity> ProductPriceGroupLevel { get; set; }
	public DbSet<OrderOneTimeInformationEntity> OrderOneTimeInformation { get; set; }
	public DbSet<KomercijalnoWebProductLinkEntity> KomercijalnoWebProductLinks { get; set; }
	public DbSet<UserPermissionEntity> UserPermissions { get; set; }
	public DbSet<CalculatorItemEntity> CalculatorItems { get; set; }
	public DbSet<ModuleHelpEntity> ModuleHelps { get; set; }

	protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
	{
		base.OnConfiguring(optionsBuilder);
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

		var stringBuilder = new NpgsqlConnectionStringBuilder
		{
			Host = configurationRoot["POSTGRES_HOST"],
			Port = int.Parse(configurationRoot["POSTGRES_PORT"]!),
			Username = configurationRoot["POSTGRES_USER"],
			Password = configurationRoot["POSTGRES_PASSWORD"],
			Database = $"{configurationRoot["DEPLOY_ENV"]}_web",
			Pooling = false,
			MinPoolSize = 1,
			MaxPoolSize = 20,
			Timeout = 15,
			IncludeErrorDetail = true
		};
		optionsBuilder.UseNpgsql(
			stringBuilder.ConnectionString,
			(action) =>
			{
				action.MigrationsHistoryTable("migrations_history");
				action.MigrationsAssembly("TD.Web.Common.DbMigrations");
			}
		);
	}

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder.Entity<UnitEntity>().AddMap(new UnitEntityMap());
		modelBuilder.Entity<CityEntity>().AddMap(new CityEntityMap());
		modelBuilder.Entity<UserEntity>().AddMap(new UserEntityMap());
		modelBuilder.Entity<OrderEntity>().AddMap(new OrderEntityMap());
		modelBuilder.Entity<StoreEntity>().AddMap(new StoreEntityMap());
		modelBuilder.Entity<SettingEntity>().AddMap(new SettingEntityMap());
		modelBuilder.Entity<ProductEntity>().AddMap(new ProductEntityMap());
		modelBuilder.Entity<OrderItemEntity>().AddMap(new OrderItemEntityMap());
		modelBuilder.Entity<ProfessionEntity>().AddMap(new ProfessionEntityMap());
		modelBuilder.Entity<GlobalAlertEntity>().AddMap(new GlobalAlertEntityMap());
		modelBuilder.Entity<PaymentTypeEntity>().AddMap(new PaymentTypeEntityMap());
		modelBuilder.Entity<ProductGroupEntity>().AddMap(new ProductGroupEntityMap());
		modelBuilder.Entity<ProductPriceEntity>().AddMap(new ProductPriceEntityMap());
		modelBuilder.Entity<StatisticsItemEntity>().AddMap(new StatisticsItemEntityMap());
		modelBuilder.Entity<ProductPriceGroupEntity>().AddMap(new ProductPriceGroupEntityMap());
		modelBuilder
			.Entity<ProductPriceGroupLevelEntity>()
			.AddMap(new ProductPriceGroupLevelEntityMap());
		modelBuilder
			.Entity<OrderOneTimeInformationEntity>()
			.AddMap(new OrderOneTimeInformationEntityMap());
		modelBuilder
			.Entity<KomercijalnoWebProductLinkEntity>()
			.AddMap(new KomercijalnoWebProductLinkEntityMap());
		modelBuilder.Entity<UserPermissionEntity>().AddMap(new UserPermissionEntityMap());
		modelBuilder.Entity<CalculatorItemEntity>().AddMap(new CalculatorItemEntityMap());
		modelBuilder.Entity<ModuleHelpEntity>().AddMap(new ModuleHelpEntityMap());
	}
}
