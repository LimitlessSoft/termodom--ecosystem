using LSCore.Contracts.Exceptions;
using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class ProracunItemRepository(OfficeDbContext dbContext) : LSCoreRepositoryBase<ProracunItemEntity>(dbContext), IProracunItemRepository
{
    public void UpdateKolicina(long id, decimal kolicina)
    {
        var item = Get(id);
        item.Kolicina = kolicina;
        dbContext.SaveChanges();
    }
}
