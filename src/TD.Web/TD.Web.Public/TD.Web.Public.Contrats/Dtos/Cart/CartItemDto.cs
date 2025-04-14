using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.Cart;

public class CartItemDto
{
	public long Id { get; set; }
	public long ProductId { get; set; }
	public string Name { get; set; }
	public decimal Quantity { get; set; }
	public string Unit { get; set; }
	public decimal Price { get; set; }
	public decimal? OneAlternatePackageEquals { get; set; }
	public string? AlternateUnit { get; set; }
	public decimal VAT { get; set; }
	public decimal PriceWithVAT => Price * ((VAT + 100) / 100);
	public decimal ValueWithVAT => PriceWithVAT * Quantity;
	public ProductStockType StockType { get; set; }
}
