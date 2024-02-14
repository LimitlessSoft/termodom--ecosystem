using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Dtos.Orders
{
    public class OrdersGetDto
    {
        public string OneTimeHash { get; set; }
        public DateTime? Date { get; set; }
        public string Status { get; set; }
        public decimal ValueWithVAT { get; set; }
        public decimal DiscountValue { get; set; }
    }
}
