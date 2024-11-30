using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Common.Repository.Repository;

public class UserRepository(WebDbContext dbContext)
    : IUserRepository
{
    public IEnumerable<UserEntity> GetUsers()
    {
        return dbContext.Users.ToList();
    }

    /// <summary>
    /// Returns all inactive users which have not placed an order in the last X days.
    /// Only isActive = true users are considered.
    /// </summary>
    /// <param name="inactivityPeriod">Period in which user must not have any checked out order to be included as inactive one</param>
    /// <returns></returns>
    public List<UserEntity> GetInactiveUsers(TimeSpan inactivityPeriod)
    {
        var lastActiveDate = DateTime.UtcNow.Add(inactivityPeriod.Duration() * -1);
        return dbContext.Users
            .Include(x => x.Orders)
            .Include(x => x.ProductPriceGroupLevels)
            .Where(x => x.IsActive
                        && !x.Orders.Any(o =>
                            o.IsActive
                            && o.CheckedOutAt > lastActiveDate))
            .ToList();
    }

    public void Update(UserEntity user)
    {
        dbContext.Users.Update(user);
        dbContext.SaveChanges();
    }

    public void Update(IEnumerable<UserEntity> user)
    {
        dbContext.Users.UpdateRange(user);
        dbContext.SaveChanges();
    }
}