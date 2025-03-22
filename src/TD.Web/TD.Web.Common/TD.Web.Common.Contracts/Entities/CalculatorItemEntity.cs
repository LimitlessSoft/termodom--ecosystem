using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class CalculatorItemEntity : LSCoreEntity
{
	public long ProductId { get; set; }
	public decimal Quantity { get; set; }
	public CalculatorType CalculatorType { get; set; }
	public int Order { get; set; }
	public bool IsPrimary { get; set; }
	public bool IsHobi { get; set; }
	public bool IsStandard { get; set; }
	public bool IsProfi { get; set; }

	[NotMapped]
	public ProductEntity Product { get; set; }
}
