using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Dtos.Users;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IUserManager
    {
        string Login(UsersLoginRequest request);
        UserMeDto Me();
        UserDto GetSingle(LSCoreIdRequest request);
        LSCoreSortedAndPagedResponse<UserDto> GetMultiple(UsersGetMultipleRequest request);
        void UpdateNickname(UsersUpdateNicknameRequest request);
        UserDto Create(UsersCreateRequest request);
    }
}
