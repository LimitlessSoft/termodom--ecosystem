using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class ProracunRepository(OfficeDbContext dbContext) : LSCoreRepositoryBase<ProracunEntity>(dbContext), IProracunRepository
{
    public void UpdateState(long requestId, ProracunState requestState)
    {
        var proracun = Get(requestId);
        proracun.State = requestState;
        dbContext.SaveChanges();
    }

    public void UpdatePPID(long requestId, int? requestPpid)
    {
        var proracun = Get(requestId);
        proracun.PPID = requestPpid;
        dbContext.SaveChanges();
    }

    public void UpdateNUID(long requestId, int requestNuid)
    {
        var proracun = Get(requestId);
        proracun.NUID = requestNuid;
        dbContext.SaveChanges();
    }
}
