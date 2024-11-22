using LSCore.Contracts.Exceptions;
using Omu.ValueInjecter.Utils;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;
public class KomercijalnoIFinansijskoPoGodinamaStatusRepository(OfficeDbContext dbContext) : IKomercijalnoIFinansijskoPoGodinamaStatusRepository
{
    public KomercijalnoIFinansijskoPoGodinamaStatusEntity GetDefault()
    {
        var entity =  dbContext.KomercijalnoIFinansijskoPoGodinamaStatus.FirstOrDefault(x => x.IsDefault && x.IsActive);
        if (entity == null)
            throw new LSCoreNotFoundException();
        return entity;
    }

    public long GetDefaultId() =>
        GetDefault().Id;

    public List<KomercijalnoIFinansijskoPoGodinamaStatusEntity> GetAllStatuses()
    {
        var entities = dbContext.KomercijalnoIFinansijskoPoGodinamaStatus.Where(x => x.IsActive).ToList();
        if (entities == null)
            throw new LSCoreNotFoundException();
        return entities;
    }
}
