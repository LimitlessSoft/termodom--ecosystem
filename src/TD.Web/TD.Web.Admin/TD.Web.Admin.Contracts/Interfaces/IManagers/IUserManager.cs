using LSCore.Contracts.Http;
using TD.Web.Admin.Contracts.Requests.Users;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IUserManager
    {
        LSCoreResponse<string> Login(UserLoginRequest request);
        LSCoreResponse Register(UserRegisterRequest request);
        LSCoreResponse PromoteUser(UserPromoteRequest request);
        LSCoreResponse SetUserProductPriceGroupLevel(SetUserProductPriceGroupLevelRequest request);
    }
}
