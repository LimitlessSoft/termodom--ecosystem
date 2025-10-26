using TD.Web.Common.Contracts;
namespace TD.Web.Public.Contracts.Dtos.Cart;

public class CartSummaryDto
{
	public bool AdditionalDiscountApplied { get => ValueWithoutVAT > LegacyConstants.MaximumCartValueForDiscount; }
	public decimal ValueWithoutVAT { get; set; }
	public decimal VATValue { get; set; }
	public decimal ValueWithVAT { get; set; }
	public decimal DiscountValue { get; set; }
}
