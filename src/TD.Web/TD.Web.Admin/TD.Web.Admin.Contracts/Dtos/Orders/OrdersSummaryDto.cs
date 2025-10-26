using TD.Web.Common.Contracts;
namespace TD.Web.Admin.Contracts.Dtos.Orders
{
	public class OrdersSummaryDto {
		public bool AdditionalDiscountApplied => ValueWithoutVAT > LegacyConstants.MaximumCartValueForDiscount;
		public decimal ValueWithoutVAT { get; set; }
		public decimal VATValue { get; set; }
		public decimal ValueWithVAT
		{
			get => ValueWithoutVAT + VATValue;
		}
		public decimal DiscountValue { get; set; }
	}
}
