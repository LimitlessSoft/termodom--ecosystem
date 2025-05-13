namespace TD.Web.Common.Contracts.Requests.OrderItems;

public class ChangeOrderItemQuantityRequest
{
	public long OrderId { get; set; }
	public long ProductId { get; set; }
	public decimal Quantity { get; set; }
}
