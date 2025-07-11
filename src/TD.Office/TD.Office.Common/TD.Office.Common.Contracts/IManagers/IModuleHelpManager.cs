using TD.Office.Common.Contracts.Dtos.ModuleHelp;
using TD.Office.Common.Contracts.Requests.ModuleHelp;

namespace TD.Office.Common.Contracts.IManagers;

public interface IModuleHelpManager
{
	ModuleHelpDto GetModuleHelps(GetModuleHelpRequest request);
	void PutModuleHelps(PutModuleHelpRequest request);
}
