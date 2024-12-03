using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.ModuleHelp;

public class PutModuleHelpRequest : LSCoreSaveRequest
{
    public ModuleType Module { get; set; }
    public string Text { get; set; }
}
