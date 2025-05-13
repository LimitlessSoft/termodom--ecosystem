using LSCore.Contracts.Requests;
using TD.TDOffice.Contracts.Dtos.Users;

namespace TD.TDOffice.Contracts.IManagers;

public interface IUserManager
{
	UserDto Get(LSCoreIdRequest request);
	List<UserDto> GetMultiple();
}
