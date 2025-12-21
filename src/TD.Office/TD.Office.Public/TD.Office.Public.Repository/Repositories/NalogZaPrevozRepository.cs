using LSCore.Repository;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class NalogZaPrevozRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<NalogZaPrevozEntity>(dbContext),
		INalogZaPrevozRepository
{
	public int CountCancelledLast7Days(int storeId)
	{
		var now = DateTime.UtcNow;
		var weekEnd = now.AddDays(7);
		return dbContext.NaloziZaPrevoz.Count(x =>
			x.StoreId == storeId
			&& x.Status == NalogZaPrevozStatus.Cancelled
			&& x.UpdatedAt >= now
			&& x.UpdatedAt < weekEnd
		);
	}
}
