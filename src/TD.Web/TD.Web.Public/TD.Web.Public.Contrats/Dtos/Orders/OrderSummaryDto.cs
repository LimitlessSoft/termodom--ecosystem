namespace TD.Web.Public.Contracts.Dtos.Orders;

public class OrderSummaryDto
{
	public decimal ValueWithoutVAT { get; set; }
	public decimal VATValue { get; set; }
	public decimal ValueWithVAT
	{
		get => ValueWithoutVAT + VATValue;
	}
	public decimal DiscountValue { get; set; }
}
