using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class ProductEntity : LSCoreEntity
{
	public string Name { get; set; }
	public string Src { get; set; }
	public string Image { get; set; }
	public string? CatalogId { get; set; }
	public long UnitId { get; set; }
	public long? AlternateUnitId { get; set; }

	/// <summary>
	/// Represents the group link identifier for products that are variations of other products.
	/// </summary>
	public string? Link { get; set; }

	/// <summary>
	/// Represents the index of the product within its linked group of variations.
	/// </summary>
	public short LinkIndex { get; set; }

	/// <summary>
	/// Text of the item used within the linked products group
	/// </summary>
	public string? LinkText { get; set; }

	/// <summary>
	/// Property which indicates how many of the alternate unit equals one of the main unit
	/// </summary>
	public decimal? OneAlternatePackageEquals { get; set; }
	public ProductClassification Classification { get; set; }
	public decimal VAT { get; set; }
	public long PriceId { get; set; }
	public long ProductPriceGroupId { get; set; }
	public string? Description { get; set; }
	public string? ShortDescription { get; set; }
	public int PriorityIndex { get; set; }
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public ProductStatus Status { get; set; }
	public ProductStockType StockType { get; set; }
	public List<string>? SearchKeywords { get; set; }

	[NotMapped]
	public UnitEntity Unit { get; set; }

	[NotMapped]
	public UnitEntity? AlternateUnit { get; set; }

	[NotMapped]
	public ProductPriceEntity Price { get; set; }

	[NotMapped]
	public List<ProductGroupEntity> Groups { get; set; }

	[NotMapped]
	public ProductPriceGroupEntity ProductPriceGroup { get; set; }

	/// <summary>
	/// Users who can manage this specific product (inverse navigation for UserEntity.ManagingProducts)
	/// </summary>
	public List<UserEntity>? ManagingUsers { get; set; }
}
