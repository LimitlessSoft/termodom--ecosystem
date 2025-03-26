using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.ModuleHelp;

public class PutModuleHelpRequest
{
	public long? Id { get; set; }
	public ModuleType Module { get; set; }
	public string Text { get; set; }
}
