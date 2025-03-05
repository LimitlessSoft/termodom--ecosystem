using TD.Web.Admin.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Orders;

public class OrdersPostForwardToKomercijalnoRequest
{
    public string OneTimeHash { get; set; }
    public ForwardToKomercijalnoType Type { get; set; }
}