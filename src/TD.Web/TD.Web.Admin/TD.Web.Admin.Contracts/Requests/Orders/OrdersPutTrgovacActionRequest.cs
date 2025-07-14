using TD.Web.Common.Contracts.Enums;
namespace TD.Web.Admin.Contracts.Requests.Orders;

public class OrdersPutTrgovacActionRequest {
    public string OneTimeHash { get; set; }
    public OrderTrgovacAction TrgovacAction { get; set; }
}