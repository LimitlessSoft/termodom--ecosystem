using LSCore.Contracts.Requests;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IUserRepository
{
    UserEntity Get(LSCoreIdRequest request);
}
