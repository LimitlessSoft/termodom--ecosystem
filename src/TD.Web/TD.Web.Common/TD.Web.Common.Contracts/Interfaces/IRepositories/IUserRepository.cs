using LSCore.Contracts.Interfaces.Repositories;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface IUserRepository : ILSCoreRepositoryBase<UserEntity>
{
    IQueryable<UserEntity> GetMultiple(bool includeInactive);
    List<UserEntity> GetInactiveUsers(TimeSpan inactivityPeriod);
}