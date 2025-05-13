namespace TD.Web.Public.Contracts.Dtos.Calculator;

public class CalculatorItemDto
{
	public string ProductName { get; set; }
	public string Unit { get; set; }
	public decimal Quantity { get; set; }
	public bool IsPrimary { get; set; }
}
