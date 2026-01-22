using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Interfaces.IRepositories;

public interface ITicketRepository : ILSCoreRepositoryBase<TicketEntity>
{
	List<TicketEntity> GetFiltered(TicketType? type, TicketStatus? status, TicketPriority? priority, long? submittedByUserId);
}
