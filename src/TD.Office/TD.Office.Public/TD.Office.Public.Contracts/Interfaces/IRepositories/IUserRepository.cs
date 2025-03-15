using LSCore.Contracts.Interfaces.Repositories;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IUserRepository : ILSCoreRepositoryBase<UserEntity>
{
	/// <summary>
	/// Returns currently authenticated user's entity.
	/// If user is not authenticated, throws unauthenticated.
	/// If user is authenticated, but not found in database or is found but isActive = false, throws not found.
	/// </summary>
	/// <returns></returns>
	UserEntity GetCurrentUser();
	void UpdateNickname(long requestId, string requestNickname);
	bool HasPermission(long userId, Permission permission);
}
