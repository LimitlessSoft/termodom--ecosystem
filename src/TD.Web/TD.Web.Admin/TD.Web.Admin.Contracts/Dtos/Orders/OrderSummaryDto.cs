namespace TD.Web.Admin.Contracts.Dtos.Orders
{
    public class OrderSummaryDto
    {
        public decimal ValueWithoutVAT { get; set; }
        public decimal VATValue { get; set; }
        public decimal ValueWithVAT { get; set; }
        public decimal DiscountValue { get; set; }
    }
}
