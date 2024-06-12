namespace TD.Web.Common.Contracts.Requests.OrderItems;

public class OrderItemExistsRequest
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
}