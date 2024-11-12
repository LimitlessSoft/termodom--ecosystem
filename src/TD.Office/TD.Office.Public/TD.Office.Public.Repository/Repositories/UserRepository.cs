using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class UserRepository(OfficeDbContext dbContext,
    LSCoreContextUser contextUser)
    : IUserRepository
{
    /// <inheritdoc />
    public UserEntity GetCurrentUser()
    {
        if(contextUser.Id == null)
            throw new LSCoreUnauthenticatedException();
        
        var user = dbContext.Users.FirstOrDefault(x => x.IsActive && x.Id == contextUser.Id);
        if(user == null)
            throw new LSCoreNotFoundException();

        return user;
    }

    public UserEntity Get(LSCoreIdRequest request) =>
        dbContext.Users.First(x => x.IsActive && x.Id == request.Id);
}
