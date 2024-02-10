namespace TD.Web.Admin.Contracts.Dtos.Orders
{
    public class OrdersGetDto
    {
        public string? OneTimeHash { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string Status { get; set; }
        public string User { get; set; }
        public decimal ValueWithVAT { get; set; }
        public decimal DiscountValue { get; set; }
    }
}
