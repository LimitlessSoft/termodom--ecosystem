using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class OdsustvoRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<OdsustvoEntity>(dbContext),
		IOdsustvoRepository
{
	public List<OdsustvoEntity> GetByDateRange(DateTime startDate, DateTime endDate, long? userId = null)
	{
		var query = dbContext.Odsustva
			.Include(x => x.User)
			.Include(x => x.TipOdsustva)
			.Where(x => x.IsActive);

		if (userId.HasValue)
		{
			query = query.Where(x => x.UserId == userId.Value);
		}

		return query
			.Where(x =>
				(x.DatumOd <= endDate && x.DatumDo >= startDate))
			.OrderBy(x => x.DatumOd)
			.ToList();
	}

}
