using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class UslovFormiranjaWebCeneRepository(OfficeDbContext dbContext)
    : IUslovFormiranjaWebCeneRepository
{
    public IQueryable<UslovFormiranjaWebCeneEntity> GetMultiple() =>
        dbContext.UsloviFormiranjaWebcena.Where(x => x.IsActive);

    public void Create(UslovFormiranjaWebCeneEntity uslov)
    {
        dbContext.UsloviFormiranjaWebcena.Add(uslov);
        dbContext.SaveChanges();
    }

    public void UpdateOrCreate(UslovFormiranjaWebCeneEntity entity)
    {
        var existing = dbContext.UsloviFormiranjaWebcena.Find(entity.Id);
        if (existing == null)
            Create(entity);
        else
        {
            dbContext.Entry(existing).CurrentValues.SetValues(entity);
            dbContext.SaveChanges();
        }
    }
}
