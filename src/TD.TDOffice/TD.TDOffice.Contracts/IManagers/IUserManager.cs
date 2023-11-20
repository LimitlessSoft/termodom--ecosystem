using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using TD.TDOffice.Contracts.Dtos.Users;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IUserManager
    {
        LSCoreResponse<UserDto> Get(LSCoreIdRequest request);
        LSCoreListResponse<UserDto> GetMultiple();
    }
}
