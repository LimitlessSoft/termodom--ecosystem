using LSCore.Auth.Contracts;
using LSCore.Auth.UserPass.Contracts;
using LSCore.Exceptions;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class UserRepository(
	OfficeDbContext dbContext,
	LSCoreAuthContextEntity<string> contextEntity
)
	: LSCoreRepositoryBase<UserEntity>(dbContext),
		IUserRepository,
		ILSCoreAuthUserPassIdentityEntityRepository<string>
{
	/// <inheritdoc />
	public UserEntity GetCurrentUser()
	{
		var user = dbContext
			.Users.Include(x => x.Permissions)
			.FirstOrDefault(x => x.IsActive && x.Username == contextEntity.Identifier);
		if (user == null)
			throw new LSCoreNotFoundException();

		return user;
	}

	public void UpdateNickname(long requestId, string requestNickname)
	{
		var user = Get(requestId);
		user.Nickname = requestNickname;
		dbContext.SaveChanges();
	}

	public bool HasPermission(long userId, Permission permission) =>
		dbContext
			.Users.Include(x => x.Permissions)
			.FirstOrDefault(x => x.IsActive && x.Id == userId)
			?.Permissions?.FirstOrDefault(x => x.Permission == permission)
			?.Permission == permission;

	public ILSCoreAuthUserPassEntity<string>? GetOrDefault(string identifier) =>
		dbContext.Users.FirstOrDefault(x => x.IsActive && x.Username == identifier);

	public void SetRefreshToken(string entityIdentifier, string refreshToken)
	{
		var user = dbContext.Users.FirstOrDefault(x =>
			x.IsActive && x.Username == entityIdentifier
		);
		if (user == null)
			throw new LSCoreNotFoundException();
		user.RefreshToken = refreshToken;
		dbContext.SaveChanges();
	}
}
