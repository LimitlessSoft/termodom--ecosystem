using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface IModuleHelpRepository
{
    ModuleHelpEntity Get(ModuleType type, long createdBy);
    ModuleHelpEntity? GetOrDefault(ModuleType type, long createdBy);
}
