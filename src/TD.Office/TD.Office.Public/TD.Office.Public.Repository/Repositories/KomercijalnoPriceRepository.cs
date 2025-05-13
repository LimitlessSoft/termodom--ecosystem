using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class KomercijalnoPriceRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<KomercijalnoPriceEntity>(dbContext),
		IKomercijalnoPriceRepository
{
	public void HardClear()
	{
		dbContext.KomercijalnoPrices.RemoveRange(dbContext.KomercijalnoPrices); // This is slow, truncate should be used
		dbContext.SaveChanges();
	}
}
