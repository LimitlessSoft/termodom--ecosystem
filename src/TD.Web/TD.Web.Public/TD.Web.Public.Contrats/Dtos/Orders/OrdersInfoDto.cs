namespace TD.Web.Public.Contracts.Dtos.Orders;

public class OrdersInfoDto
{
	public string User { get; set; }
	public int NumberOfOrders { get; set; }
	public decimal TotalDiscountValue { get; set; }
}
