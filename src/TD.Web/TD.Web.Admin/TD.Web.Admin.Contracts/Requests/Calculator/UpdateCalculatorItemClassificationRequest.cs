using LSCore.Common.Contracts;

namespace TD.Web.Admin.Contracts.Requests.Calculator;

public class UpdateCalculatorItemClassificationRequest : LSCoreIdRequest
{
	public bool IsHobi { get; set; }
	public bool IsStandard { get; set; }
	public bool IsProfi { get; set; }
}
