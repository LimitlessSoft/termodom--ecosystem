using LSCore.Auth.Contracts;
using LSCore.Auth.UserPass.Contracts;
using LSCore.Exceptions;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class UserRepository(WebDbContext dbContext, LSCoreAuthContextEntity<string> contextEntity)
	: LSCoreRepositoryBase<UserEntity>(dbContext),
		IUserRepository,
		ILSCoreAuthUserPassIdentityEntityRepository<string>
{
	public UserEntity GetCurrentUser()
	{
		if (!contextEntity.IsAuthenticated)
			throw new LSCoreUnauthenticatedException();

		var user = dbContext.Users.FirstOrDefault(x =>
			x.IsActive && x.Username == contextEntity.Identifier
		);
		if (user == null)
			throw new LSCoreNotFoundException();

		return user;
	}

	public IQueryable<UserEntity> GetMultiple(bool includeInactive)
	{
		var query = dbContext.Users.AsQueryable();
		if (!includeInactive)
			query = query.Where(x => x.IsActive);
		return query;
	}

	public List<UserEntity> GetInactiveUsers(TimeSpan inactivityPeriod)
	{
		var lastActiveDate = DateTime.UtcNow.Add(inactivityPeriod.Duration() * -1);
		return dbContext
			.Users.Include(x => x.Orders)
			.Include(x => x.ProductPriceGroupLevels)
			.Where(x =>
				x.IsActive && !x.Orders.Any(o => o.IsActive && o.CheckedOutAt > lastActiveDate)
			)
			.ToList();
	}

	public UserEntity GetByUsername(string username)
	{
		var user = dbContext.Users.FirstOrDefault(x => x.IsActive && x.Username == username);
		if (user == null)
			throw new LSCoreNotFoundException();
		return user;
	}

	public ILSCoreAuthUserPassEntity<string>? GetOrDefault(string identifier) =>
		dbContext.Users.FirstOrDefault(x =>
			x.Username.ToLower() == identifier.ToLower() && x.IsActive
		);

	public void SetRefreshToken(string entityIdentifier, string refreshToken)
	{
		var user = GetOrDefault(entityIdentifier);
		if (user == null)
			throw new LSCoreNotFoundException();
		user.RefreshToken = refreshToken;
		dbContext.SaveChanges();
	}
}
