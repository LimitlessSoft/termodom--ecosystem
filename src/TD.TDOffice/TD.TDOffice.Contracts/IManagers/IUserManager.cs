using TD.TDOffice.Contracts.Dtos.Users;
using LSCore.Contracts.Requests;

namespace TD.TDOffice.Contracts.IManagers;

public interface IUserManager
{
    UserDto Get(LSCoreIdRequest request);
    List<UserDto> GetMultiple();
}