using LSCore.Contracts.Exceptions;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class ProracunItemRepository(OfficeDbContext dbContext) : IProracunItemRepository
{
    public ProracunItemEntity? GetOrDefault(long id) =>
        dbContext.ProracunItems.FirstOrDefault(x => x.Id == id);

    public ProracunItemEntity Get(long id)
    {
        var u = GetOrDefault(id);
        if (u == null)
            throw new LSCoreNotFoundException();

        return u;
    }

    public void UpdateKolicina(long id, decimal kolicina)
    {
        var item = Get(id);
        item.Kolicina = kolicina;
        dbContext.SaveChanges();
    }

    public void Update(ProracunItemEntity item)
    {
        dbContext.ProracunItems.Update(item);
        dbContext.SaveChanges();
    }
}
