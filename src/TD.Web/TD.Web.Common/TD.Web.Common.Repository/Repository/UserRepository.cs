using TD.Web.Common.Contracts.Interfaces.IRepositories;
using LSCore.Contracts.Interfaces.Repositories;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;

namespace TD.Web.Common.Repository.Repository;

public class UserRepository(WebDbContext dbContext)
    : LSCoreRepositoryBase<UserEntity>(dbContext), IUserRepository, ILSCoreAuthorizableEntityRepository
{
    public ILSCoreAuthorizable? Get(string username) =>
        dbContext.Users.FirstOrDefault(x => x.IsActive && x.Username == username);

    public void SetRefreshToken(long id, string refreshToken)
    {
        var user = Get(id);
        user.RefreshToken = refreshToken;
        dbContext.SaveChanges();
    }

    public ILSCoreAuthorizable? GetByRefreshToken(string refreshToken) =>
        dbContext.Users.FirstOrDefault(x => x.IsActive && x.RefreshToken == refreshToken);

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
        return dbContext.Users
            .Include(x => x.Orders)
            .Include(x => x.ProductPriceGroupLevels)
            .Where(x => x.IsActive
                        && !x.Orders.Any(o =>
                            o.IsActive
                            && o.CheckedOutAt > lastActiveDate))
            .ToList();
    }
}