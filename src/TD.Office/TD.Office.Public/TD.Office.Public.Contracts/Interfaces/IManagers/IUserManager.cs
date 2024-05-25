using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Dtos.Users;
using TD.Office.Public.Contracts.Requests.Users;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IUserManager : ILSCoreBaseManager
    {
        LSCoreResponse<string> Login(UsersLoginRequest request);
        LSCoreResponse<UserMeDto> Me();
        LSCoreResponse<UserDto> GetSingle(LSCoreIdRequest request);
        LSCoreSortedPagedResponse<UserDto> GetMultiple(UsersGetMultipleRequest request);
        LSCoreResponse UpdateNickname(UsersUpdateNicknameRequest request);
        LSCoreResponse<UserDto> Create(UsersCreateRequest request);
    }
}
