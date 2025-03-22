using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface IUserRepository : ILSCoreRepositoryBase<UserEntity>
{
	UserEntity GetCurrentUser();
	IQueryable<UserEntity> GetMultiple(bool includeInactive);
	List<UserEntity> GetInactiveUsers(TimeSpan inactivityPeriod);
	UserEntity GetByUsername(string username);
}
