using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class KomercijalnoPriceRepository(OfficeDbContext dbContext) : IKomercijalnoPriceRepository
{
    public IQueryable<KomercijalnoPriceEntity> GetMultiple() =>
        dbContext.KomercijalnoPrices.Where(x => x.IsActive);

    public void HardClear()
    {
        dbContext.KomercijalnoPrices.RemoveRange(dbContext.KomercijalnoPrices); // This is slow, truncate should be used
        dbContext.SaveChanges();
    }

    public void Create(List<KomercijalnoPriceEntity> list)
    {
        dbContext.KomercijalnoPrices.AddRange(list);
        dbContext.SaveChanges();
    }
}
