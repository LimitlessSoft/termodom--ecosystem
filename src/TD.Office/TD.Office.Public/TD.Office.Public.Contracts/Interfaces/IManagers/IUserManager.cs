using TD.Office.Common.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Requests.Users;
using TD.Office.Public.Contracts.Dtos.Users;
using LSCore.Contracts.Responses;
using LSCore.Contracts.Requests;
using TD.Office.Public.Contracts.Dtos.Permissions;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IUserManager
{
    string Login(UsersLoginRequest request);
    UserMeDto Me();
    UserDto GetSingle(LSCoreIdRequest request);
    LSCoreSortedAndPagedResponse<UserDto> GetMultiple(UsersGetMultipleRequest request);
    void UpdateNickname(UsersUpdateNicknameRequest request);
    UserDto Create(UsersCreateRequest request);
    List<PermissionDto> GetPermissions(LSCoreIdRequest request);
    void UpdatePermission(UsersUpdatePermissionRequest request);
}