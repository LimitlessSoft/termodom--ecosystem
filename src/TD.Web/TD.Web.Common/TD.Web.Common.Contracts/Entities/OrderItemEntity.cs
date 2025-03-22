using System.ComponentModel.DataAnnotations.Schema;
using LSCore.Repository.Contracts;

namespace TD.Web.Common.Contracts.Entities;

public class OrderItemEntity : LSCoreEntity
{
	public decimal Price { get; set; }
	public decimal Quantity { get; set; }
	public decimal PriceWithoutDiscount { get; set; }
	public decimal VAT { get; set; }
	public long OrderId { get; set; }
	public long ProductId { get; set; }

	[NotMapped]
	public OrderEntity Order { get; set; }

	[NotMapped]
	public ProductEntity Product { get; set; }
}
