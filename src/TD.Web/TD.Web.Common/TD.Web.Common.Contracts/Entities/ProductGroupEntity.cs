using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class ProductGroupEntity : LSCoreEntity
{
	public string Name { get; set; }
	public long? ParentGroupId { get; set; }
	public string? WelcomeMessage { get; set; }
	public ProductGroupType Type { get; set; }
	public string? SalesMobile { get; set; }
	public string Src { get; set; }

	/// <summary>
	/// Used to determine which user can edit which product group & products inside it
	/// </summary>
	public List<UserEntity>? ManagingUsers { get; set; } // Leaving it to EF to map many to many

	[NotMapped]
	public List<ProductEntity> Products { get; set; }

	[NotMapped]
	public ProductGroupEntity? ParentGroup { get; set; }
}
