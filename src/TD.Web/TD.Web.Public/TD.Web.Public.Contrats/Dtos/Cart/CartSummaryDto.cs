namespace TD.Web.Public.Contracts.Dtos.Cart
{
    public class CartSummaryDto
    {
        public decimal TotalWithoutVAT { get; set; }
        public decimal VAT { get; set; }
        public decimal TotalWithoutDiscount { get; set; }
    }
}
