using LSCore.Contracts.Exceptions;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;
public class KomercijalnoIFinansijskoPoGodinamaRepository(OfficeDbContext dbContext) : IKomercijalnoIFinansijskoPoGodinamaRepository
{
    public KomercijalnoIFinansijskoPoGodinamaEntity GetByPPID(int PPID)
    {
        var entities = dbContext.KomercijalnoIFinansijskoPoGodinama.Where(x => x.IsActive && x.PPID == PPID).FirstOrDefault();
        if (entities == null)
            throw new LSCoreNotFoundException();
        return entities;
    }
}
