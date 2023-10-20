using TD.Core.Contracts.Http;
using TD.Web.Contracts.Requests.Users;

namespace TD.Web.Contracts.Interfaces.IManagers
{
    public interface IUserManager
    {
        Response<string> Login(UserLoginRequest request);
        Response Register(UserRegisterRequest request);
        Response<bool> PromoteUser(UserPromoteRequest request);
    }
}
