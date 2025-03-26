using LSCore.Exceptions;
using LSCore.Repository;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class ModuleHelpRepository(WebDbContext dbContext)
	: LSCoreRepositoryBase<ModuleHelpEntity>(dbContext),
		IModuleHelpRepository
{
	/// <summary>
	/// Gets module help by type and createdBy.
	/// Pass createdBy = 0 to get system help.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="createdBy"></param>
	/// <returns></returns>
	public ModuleHelpEntity Get(ModuleType type, long createdBy)
	{
		var entity = GetOrDefault(type, createdBy);
		if (entity == null)
			throw new LSCoreNotFoundException();

		return entity;
	}

	/// <summary>
	/// Gets module help by type and createdBy.
	/// Pass createdBy = 0 to get system help.
	/// </summary>
	/// <param name="type"></param>
	/// <param name="createdBy"></param>
	/// <returns></returns>
	public ModuleHelpEntity? GetOrDefault(ModuleType type, long createdBy) =>
		dbContext.ModuleHelps.FirstOrDefault(x =>
			x.IsActive && x.ModuleType == type && x.CreatedBy == createdBy
		);
}
