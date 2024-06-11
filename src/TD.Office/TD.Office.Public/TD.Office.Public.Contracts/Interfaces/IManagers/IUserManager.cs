using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Dtos.Users;
using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IUserManager
    {
        string Login(UsersLoginRequest request);
        UserMeDto Me();
        UserDto GetSingle(LSCoreIdRequest request);
        List<UserDto> GetMultiple(UsersGetMultipleRequest request);
        void UpdateNickname(UsersUpdateNicknameRequest request);
        UserDto Create(UsersCreateRequest request);
    }
}
