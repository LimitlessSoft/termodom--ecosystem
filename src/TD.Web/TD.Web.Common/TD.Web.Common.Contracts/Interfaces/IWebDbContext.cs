using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces;

public interface IWebDbContext : IDisposable
{
	DbSet<UnitEntity> Units { get; set; }
	DbSet<UserEntity> Users { get; set; }
	DbSet<CityEntity> Cities { get; set; }
	DbSet<OrderEntity> Orders { get; set; }
	DbSet<StoreEntity> Stores { get; set; }
	DbSet<ProductEntity> Products { get; set; }
	DbSet<SettingEntity> Settings { get; set; }
	DbSet<OrderItemEntity> OrderItems { get; set; }
	DbSet<ProfessionEntity> Professions { get; set; }
	DbSet<GlobalAlertEntity> GlobalAlerts { get; set; }
	DbSet<PaymentTypeEntity> PaymentTypes { get; set; }
	DbSet<ProductGroupEntity> ProductGroups { get; set; }
	DbSet<ProductPriceEntity> ProductPrices { get; set; }
	DbSet<StatisticsItemEntity> StatisticsItems { get; set; }
	DbSet<ProductPriceGroupEntity> ProductPriceGroups { get; set; }
	DbSet<ProductPriceGroupLevelEntity> ProductPriceGroupLevel { get; set; }
	DbSet<OrderOneTimeInformationEntity> OrderOneTimeInformation { get; set; }
	DbSet<KomercijalnoWebProductLinkEntity> KomercijalnoWebProductLinks { get; set; }
	DbSet<UserPermissionEntity> UserPermissions { get; set; }
	DbSet<CalculatorItemEntity> CalculatorItems { get; set; }
	DbSet<ModuleHelpEntity> ModuleHelps { get; set; }
}
