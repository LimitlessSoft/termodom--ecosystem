using TD.Core.Contracts.Requests;
using TD.Web.Veleprodaja.Contracts.Enums.Orders;

namespace TD.Web.Veleprodaja.Contracts.Requests.Orders
{
    public class OrdersPutRequest : SaveRequest
    {
        public int UserId { get; set; }
        public OrderStatus Status { get; set; }
    }
}
