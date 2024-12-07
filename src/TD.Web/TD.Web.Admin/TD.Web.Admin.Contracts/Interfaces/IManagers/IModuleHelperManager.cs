using TD.Web.Admin.Contracts.Dtos.ModuleHelper;
using TD.Web.Admin.Contracts.Requests.ModuleHelp;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IModuleHelperManager
{
    ModuleHelpDto GetModuleHelps(GetModuleHelpRequest request);
    void PutModuleHelps(PutModuleHelpRequest request);
}
