using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Contracts.Requests.Calculator;

public class GetCalculatorItemsRequest
{
	public CalculatorType Type { get; set; }
}
