using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Auth.UserPass.Contracts;
using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class UserEntity : LSCoreEntity, ILSCoreAuthUserPassEntity<string>
{
	public long CityId { get; set; }
	public string? Mail { get; set; }
	public string Mobile { get; set; }
	public UserType Type { get; set; }
	public string Address { get; set; }
	public long? ReferentId { get; set; }
	public string Username { get; set; }
	public string Password { get; set; }
	public string? RefreshToken { get; set; }
	public string Nickname { get; set; }
	public long FavoriteStoreId { get; set; }
	public DateTime DateOfBirth { get; set; }
	public DateTime? LastTimeSeen { get; set; }
	public DateTime? ProcessingDate { get; set; }
	public long DefaultPaymentTypeId { get; set; }
	public long? ProfessionId { get; set; }
	public string? PIB { get; set; }
	public int? PPID { get; set; }
	public string? Comment { get; set; }

	/// <summary>
	/// Used to determine which user can edit which product group & products inside it
	/// </summary>
	public List<ProductGroupEntity>? ManaginProductGroups { get; set; } // Leaving it to EF to map many to many

	[NotMapped]
	public PaymentTypeEntity? DefaultPaymentType { get; set; }

	[NotMapped]
	public List<UserPermissionEntity> Permissions { get; set; }

	[NotMapped]
	public List<OrderEntity> Orders { get; set; }

	[NotMapped]
	public List<ProductPriceGroupLevelEntity> ProductPriceGroupLevels { get; set; }

	[NotMapped]
	public CityEntity City { get; set; }

	[NotMapped]
	public StoreEntity FavoriteStore { get; set; }

	[NotMapped]
	public ProfessionEntity? Profession { get; set; }

	[NotMapped]
	public UserEntity? Referent { get; set; }

	public string Identifier => Username;
}
