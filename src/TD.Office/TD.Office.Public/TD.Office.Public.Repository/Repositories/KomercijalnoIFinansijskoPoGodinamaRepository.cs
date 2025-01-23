using LSCore.Contracts.Exceptions;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class KomercijalnoIFinansijskoPoGodinamaRepository(OfficeDbContext dbContext)
    : IKomercijalnoIFinansijskoPoGodinamaRepository
{
    public KomercijalnoIFinansijskoPoGodinamaEntity Get(long id)
    {
        var entity = GetOrDefault(id);
        if (entity is null)
            throw new LSCoreNotFoundException();

        return entity;
    }

    public KomercijalnoIFinansijskoPoGodinamaEntity? GetOrDefault(long id) =>
        dbContext.KomercijalnoIFinansijskoPoGodinama.FirstOrDefault(x => x.IsActive && x.Id == id);

    public KomercijalnoIFinansijskoPoGodinamaEntity GetByPPID(int PPID)
    {
        var entities = dbContext.KomercijalnoIFinansijskoPoGodinama.FirstOrDefault(x =>
            x.IsActive && x.PPID == PPID
        );
        if (entities == null)
            throw new LSCoreNotFoundException();
        return entities;
    }

    public KomercijalnoIFinansijskoPoGodinamaEntity Create(int ppid, long statusDefaultId)
    {
        var entity = new KomercijalnoIFinansijskoPoGodinamaEntity
        {
            PPID = ppid,
            StatusId = statusDefaultId
        };
        dbContext.KomercijalnoIFinansijskoPoGodinama.Add(entity);
        dbContext.SaveChanges();
        return entity;
    }

    public void Update(KomercijalnoIFinansijskoPoGodinamaEntity entity)
    {
        dbContext.KomercijalnoIFinansijskoPoGodinama.Update(entity);
        dbContext.SaveChanges();
    }
}
