using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Responses;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Requests.Users;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IUserManager : ILSCoreBaseManager
    {
        LSCoreResponse<string> Login(UserLoginRequest request);
        LSCoreResponse Register(UserRegisterRequest request);
        LSCoreResponse PromoteUser(UserPromoteRequest request);
        LSCoreResponse SetUserProductPriceGroupLevel(SetUserProductPriceGroupLevelRequest request);
        LSCoreResponse MarkLastSeen();
        LSCoreResponse<UserInformationDto> Me();
        LSCoreSortedPagedResponse<UsersGetDto> GetUsers(UsersGetRequest request);
        LSCoreResponse<GetSingleUserDto> GetSingleUser(GetSingleUserRequest request);
    }
}
