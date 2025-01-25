using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface IUslovFormiranjaWebCeneRepository
{
    IQueryable<UslovFormiranjaWebCeneEntity> GetMultiple();
    void Create(UslovFormiranjaWebCeneEntity uslov);
    void UpdateOrCreate(UslovFormiranjaWebCeneEntity entity);
}
