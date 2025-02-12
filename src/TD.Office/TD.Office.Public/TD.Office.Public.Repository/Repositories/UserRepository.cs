using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Interfaces;
using LSCore.Contracts.Interfaces.Repositories;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class UserRepository(OfficeDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreRepositoryBase<UserEntity>(dbContext), IUserRepository, ILSCoreAuthorizableEntityRepository
{
    /// <inheritdoc />
    public UserEntity GetCurrentUser()
    {
        if (contextUser.Id == null)
            throw new LSCoreUnauthenticatedException();

        var user = dbContext.Users
            .Include(x => x.Permissions)
            .FirstOrDefault(x => x.IsActive && x.Id == contextUser.Id);
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

    public ILSCoreAuthorizable? Get(string username) =>
        dbContext.Users.FirstOrDefault(x => x.IsActive && x.Username.ToLower() == username.ToLower());

    public void SetRefreshToken(long id, string refreshToken)
    {
        var user = Get(id);
        user.RefreshToken = refreshToken;
        dbContext.SaveChanges();
    }

    public ILSCoreAuthorizable? GetByRefreshToken(string refreshToken) =>
        dbContext.Users.FirstOrDefault(x => x.IsActive && x.RefreshToken == refreshToken);
}
