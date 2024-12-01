using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IRepositories;

public interface IUserRepository
{
    IEnumerable<UserEntity> GetUsers();
    List<UserEntity> GetInactiveUsers(TimeSpan inactivityPeriod);
    void Update(UserEntity user);
    void Update(IEnumerable<UserEntity> user);
}