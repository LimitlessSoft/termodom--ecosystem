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
	public List<TicketEntity> GetFiltered(TicketType? type, TicketStatus? status, TicketPriority? priority, long? submittedByUserId)
	{
		var query = dbContext.Tickets
			.Include(x => x.SubmittedByUser)
			.Where(x => x.IsActive);

		if (type.HasValue)
		{
			query = query.Where(x => x.Type == type.Value);
		}

		if (status.HasValue)
		{
			query = query.Where(x => x.Status == status.Value);
		}

		if (priority.HasValue)
		{
			query = query.Where(x => x.Priority == priority.Value);
		}

		if (submittedByUserId.HasValue)
		{
			query = query.Where(x => x.SubmittedByUserId == submittedByUserId.Value);
		}

		return query
			.OrderByDescending(x => x.CreatedAt)
			.ToList();
	}
}
