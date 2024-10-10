using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Calculator;

public class AddCalculatorItemRequest
{
    public long ProductId { get; set; }
    public decimal Quantity { get; set; }
    public CalculatorType CalculatorType { get; set; }
    public bool IsPrimary { get; set; }
}
