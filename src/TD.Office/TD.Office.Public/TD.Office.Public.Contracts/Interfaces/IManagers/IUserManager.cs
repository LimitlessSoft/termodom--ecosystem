using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Office.Common.Contracts.Requests.Users;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface IUserManager : ILSCoreBaseManager
    {
        LSCoreResponse<string> Login(UsersLoginRequest request);
    }
}
