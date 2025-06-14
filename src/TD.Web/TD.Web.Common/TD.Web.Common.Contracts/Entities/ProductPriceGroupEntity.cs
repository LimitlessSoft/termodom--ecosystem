using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class ProductPriceGroupEntity : LSCoreEntity
{
	public string Name { get; set; }
	public ProductPriceGroupTrackUserLevel TrackUserLevel { get; set; }

	[NotMapped]
	public List<ProductEntity>? Products { get; set; }

	[NotMapped]
	public List<ProductPriceGroupLevelEntity> ProductPriceGroupLevels { get; set; }
}
