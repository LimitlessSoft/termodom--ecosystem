using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Calculator;

public class MoveCalculatorItemRequest : LSCoreIdRequest
{
    public string Direction { get; set; } // up or down
}
