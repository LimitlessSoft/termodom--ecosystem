using LSCore.Contracts.Requests;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class UserRepository(OfficeDbContext dbContext) : IUserRepository
{
    public UserEntity Get(LSCoreIdRequest request) =>
        dbContext.Users.First(x => x.Id == request.Id);
}
