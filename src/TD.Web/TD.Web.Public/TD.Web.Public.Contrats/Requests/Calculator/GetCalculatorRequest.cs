using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Requests.Calculator;

public class GetCalculatorRequest
{
	public CalculatorType Type { get; set; }
	public decimal Quantity { get; set; }
}
