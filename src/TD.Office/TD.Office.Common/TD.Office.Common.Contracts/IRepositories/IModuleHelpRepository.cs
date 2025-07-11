using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.IRepositories;

public interface IModuleHelpRepository : ILSCoreRepositoryBase<ModuleHelpEntity>
{
	ModuleHelpEntity Get(ModuleType type, long createdBy);
	ModuleHelpEntity? GetOrDefault(ModuleType type, long createdBy);
}
