using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Repository.Repositories;

public class TicketRepository(OfficeDbContext dbContext)
	: LSCoreRepositoryBase<TicketEntity>(dbContext),
		ITicketRepository
{
	public IQueryable<TicketEntity> GetFiltered(List<TicketType>? types, List<TicketStatus>? statuses, List<TicketPriority>? priorities, long? submittedByUserId)
	{
		var query = dbContext.Tickets
			.Include(x => x.SubmittedByUser)
			.Where(x => x.IsActive);

		if (types is { Count: > 0 })
		{
			query = query.Where(x => types.Contains(x.Type));
		}

		if (statuses is { Count: > 0 })
		{
			query = query.Where(x => statuses.Contains(x.Status));
		}

		if (priorities is { Count: > 0 })
		{
			query = query.Where(x => priorities.Contains(x.Priority));
		}

		if (submittedByUserId.HasValue)
		{
			query = query.Where(x => x.SubmittedByUserId == submittedByUserId.Value);
		}

		return query;
	}

	public List<TicketEntity> GetRecentlySolved(int limit)
	{
		return dbContext.Tickets
			.Include(x => x.SubmittedByUser)
			.Include(x => x.ResolvedByUser)
			.Where(x => x.IsActive && x.Status == TicketStatus.Resolved)
			.OrderByDescending(x => x.ResolvedAt)
			.Take(limit)
			.ToList();
	}
}
