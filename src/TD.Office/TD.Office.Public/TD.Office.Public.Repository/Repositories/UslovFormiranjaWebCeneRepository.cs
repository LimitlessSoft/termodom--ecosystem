using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class UslovFormiranjaWebCeneRepository(OfficeDbContext dbContext)
    : LSCoreRepositoryBase<UslovFormiranjaWebCeneEntity>(dbContext), IUslovFormiranjaWebCeneRepository
{
    public void UpdateOrCreate(UslovFormiranjaWebCeneEntity entity)
    {
        var existing = dbContext.UsloviFormiranjaWebcena.Find(entity.Id);
        if (existing == null)
            Insert(entity);
        else
            Update(entity);
    }
}
