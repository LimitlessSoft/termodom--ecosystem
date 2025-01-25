using LSCore.Contracts.Exceptions;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class NalogZaPrevozRepository(OfficeDbContext dbContext) : INalogZaPrevozRepository
{
    public NalogZaPrevozEntity Get(long id)
    {
        var entity = GetOrDefault(id);
        if (entity is null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public NalogZaPrevozEntity? GetOrDefault(long id) =>
        dbContext.NaloziZaPrevoz.FirstOrDefault(x => x.IsActive && x.Id == id);

    public void UpdateOrCreate(NalogZaPrevozEntity entity)
    {
        if (entity.Id == 0)
            dbContext.NaloziZaPrevoz.Add(entity);
        else
            dbContext.NaloziZaPrevoz.Update(entity);

        dbContext.SaveChanges();
    }

    public IQueryable<NalogZaPrevozEntity> GetMultiple() =>
        dbContext.NaloziZaPrevoz.Where(x => x.IsActive);
}
