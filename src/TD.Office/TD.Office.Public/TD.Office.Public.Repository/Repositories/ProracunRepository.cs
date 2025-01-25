using LSCore.Contracts.Exceptions;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class ProracunRepository(OfficeDbContext dbContext) : IProracunRepository
{
    // <inheritdoc />
    public ProracunEntity Get(long id)
    {
        var p = dbContext
            .Proracuni.Include(x => x.Items)
            .FirstOrDefault(x => x.Id == id && x.IsActive);

        if (p == null)
            throw new LSCoreNotFoundException();

        return p;
    }

    public void Insert(ProracunEntity proracunEntity)
    {
        dbContext.Proracuni.Add(proracunEntity);
        dbContext.SaveChanges();
    }

    public IQueryable<ProracunEntity> GetMultiple() => dbContext.Proracuni.Where(x => x.IsActive);

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

    public void Update(ProracunEntity proracun)
    {
        dbContext.Proracuni.Update(proracun);
        dbContext.SaveChanges();
    }

    public void HardDelete(long requestId)
    {
        var proracun = Get(requestId);
        dbContext.Proracuni.Remove(proracun);
        dbContext.SaveChanges();
    }
}
