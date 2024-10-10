using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Calculator;

public class UpdateCalculatorItemUnitRequest : LSCoreSaveRequest
{
    public string Unit { get; set; }
}
