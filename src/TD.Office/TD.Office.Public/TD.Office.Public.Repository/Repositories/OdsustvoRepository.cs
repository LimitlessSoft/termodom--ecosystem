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
	public List<OdsustvoEntity> GetByDateRange(DateTime startDate, DateTime endDate, int? storeId = null, long? userId = null)
	{
		var query = dbContext.Odsustva
			.Include(x => x.User)
			.Include(x => x.TipOdsustva)
			.Include(x => x.OdobrenoByUser)
			.Where(x => x.IsActive);

		if (storeId.HasValue)
		{
			// Filter by store - show all users from the same store
			query = query.Where(x => x.User != null && x.User.StoreId == storeId.Value);
		}
		else if (userId.HasValue)
		{
			// No store - show only own odsustva
			query = query.Where(x => x.UserId == userId.Value);
		}

		return query
			.Where(x =>
				(x.DatumOd <= endDate && x.DatumDo >= startDate))
			.OrderBy(x => x.DatumOd)
			.ToList();
	}

	public List<OdsustvoEntity> GetByYearAndUser(int year, long userId)
	{
		var startDate = new DateTime(year, 1, 1);
		var endDate = new DateTime(year, 12, 31);

		return dbContext.Odsustva
			.Include(x => x.User)
			.Include(x => x.TipOdsustva)
			.Include(x => x.OdobrenoByUser)
			.Where(x => x.IsActive)
			.Where(x => x.UserId == userId)
			.Where(x => x.DatumOd <= endDate && x.DatumDo >= startDate)
			.OrderBy(x => x.DatumOd)
			.ToList();
	}
}
