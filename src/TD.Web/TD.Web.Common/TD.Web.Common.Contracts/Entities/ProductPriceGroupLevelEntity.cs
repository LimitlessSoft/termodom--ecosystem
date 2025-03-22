using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;

namespace TD.Web.Common.Contracts.Entities;

public class ProductPriceGroupLevelEntity : LSCoreEntity
{
	public long UserId { get; set; }
	public int Level { get; set; }
	public long ProductPriceGroupId { get; set; }

	[NotMapped]
	public UserEntity User { get; set; }

	[NotMapped]
	public ProductPriceGroupEntity ProductPriceGroup { get; set; }
}
