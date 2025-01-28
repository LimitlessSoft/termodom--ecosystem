using LSCore.Contracts.Exceptions;
using LSCore.Repository;
using Omu.ValueInjecter.Utils;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;
public class KomercijalnoIFinansijskoPoGodinamaStatusRepository(OfficeDbContext dbContext) : LSCoreRepositoryBase<KomercijalnoIFinansijskoPoGodinamaStatusEntity>(dbContext), IKomercijalnoIFinansijskoPoGodinamaStatusRepository
{
    public long GetDefaultId() =>
        GetMultiple().First(x => x.IsDefault).Id;

    public List<KomercijalnoIFinansijskoPoGodinamaStatusEntity> GetAllStatuses()
    {
        var entities = dbContext.KomercijalnoIFinansijskoPoGodinamaStatus.Where(x => x.IsActive).ToList();
        if (entities == null)
            throw new LSCoreNotFoundException();
        return entities;
    }
}
