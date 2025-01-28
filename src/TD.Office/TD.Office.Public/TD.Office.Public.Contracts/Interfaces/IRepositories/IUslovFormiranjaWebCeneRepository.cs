using LSCore.Contracts.Interfaces.Repositories;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IUslovFormiranjaWebCeneRepository : ILSCoreRepositoryBase<UslovFormiranjaWebCeneEntity>
{
    void UpdateOrCreate(UslovFormiranjaWebCeneEntity entity);
}
