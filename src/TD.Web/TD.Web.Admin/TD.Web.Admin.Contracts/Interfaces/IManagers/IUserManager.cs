using TD.Core.Contracts.Http;
using TD.Web.Admin.Contracts.Requests.Users;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IUserManager
    {
        Response<string> Login(UserLoginRequest request);
        Response Register(UserRegisterRequest request);
        Response PromoteUser(UserPromoteRequest request);
    }
}
