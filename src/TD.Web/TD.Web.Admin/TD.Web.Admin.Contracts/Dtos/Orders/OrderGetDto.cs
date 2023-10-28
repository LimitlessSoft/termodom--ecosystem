using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Dtos.Orders
{
    public class OrderGetDto
    {

        public int UserId { get; set; }
        public int? Referent { get; set; }
        public OrderStatus Status { get; set; }
        public DateTime DateUtc { get; set; }
        public int? StoreId { get; set; }
        public int? PaymentType { get; set; }
    }
}
