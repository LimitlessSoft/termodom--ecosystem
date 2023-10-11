using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Dtos.Orders
{
    public class OrdersGetDto
    {
        public int UserId { get; set; }
        public int? Referent { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime Date { get; set; }
        public int? StoreId { get; set; }
        public int? PaymentType { get; set; }
    }
}
