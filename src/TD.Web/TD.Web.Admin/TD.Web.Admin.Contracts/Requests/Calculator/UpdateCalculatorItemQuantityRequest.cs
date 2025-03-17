namespace TD.Web.Admin.Contracts.Requests.Calculator;

public class UpdateCalculatorItemQuantityRequest
{
	public long? Id { get; set; }
	public decimal Quantity { get; set; }
}
