using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Orders
{
    public class OrdersPutStatusRequest
    {
        public string OneTimeHash { get; set; }
        public OrderStatus Status { get; set; }
    }
}