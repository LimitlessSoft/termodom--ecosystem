using TD.Core.Contracts.Http;
using TD.Core.Contracts.IManagers;
using TD.Web.Veleprodaja.Contracts.Dtos.Users;
using TD.Web.Veleprodaja.Contracts.Requests;

namespace TD.Web.Veleprodaja.Contracts.IManagers
{
    public interface IUserManager : IBaseManager
    {
        Response<string> Authenticate(UsersAuthenticateRequest request);
        Response<UsersMeDto> Me();
    }
}
