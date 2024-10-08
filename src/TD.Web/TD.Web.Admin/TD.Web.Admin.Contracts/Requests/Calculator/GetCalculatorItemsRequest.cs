using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Calculator;

public class GetCalculatorItemsRequest
{
    public CalculatorType Type { get; set; }
}
