using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Requests.ModuleHelp;

public class PutModuleHelpRequest
{
	public long? Id { get; set; }
	public ModuleType Module { get; set; }
	public string Text { get; set; }
}
