using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface ITicketRepository : ILSCoreRepositoryBase<TicketEntity>
{
	IQueryable<TicketEntity> GetFiltered(List<TicketType>? types, List<TicketStatus>? statuses, List<TicketPriority>? priorities, long? submittedByUserId);
	List<TicketEntity> GetRecentlySolved(int limit);
}
