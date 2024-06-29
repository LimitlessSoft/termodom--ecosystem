using TD.TDOffice.Contracts.DtoMappings.Users;
using TD.TDOffice.Contracts.Dtos.Users;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Managers;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Domain.Managers;

public class UserManager (ILogger<UserManager> logger, TDOfficeDbContext dbContext)
    : LSCoreManagerBase<UserManager, User>(logger, dbContext), IUserManager
{
    public UserDto Get(LSCoreIdRequest request)
    {
        var user = Queryable()
            .FirstOrDefault(x => x.Id == request.Id);
        
        if (user == null)
            throw new LSCoreNotFoundException();
        return user.ToUserDto();
    }

    public List<UserDto> GetMultiple() =>
        Queryable().ToList().ToUserDtoList();
}