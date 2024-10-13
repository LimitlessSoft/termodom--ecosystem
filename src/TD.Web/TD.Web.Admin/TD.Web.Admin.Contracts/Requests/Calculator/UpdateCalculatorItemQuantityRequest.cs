using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Calculator;

public class UpdateCalculatorItemQuantityRequest : LSCoreSaveRequest
{
    public decimal Quantity { get; set; }
}
