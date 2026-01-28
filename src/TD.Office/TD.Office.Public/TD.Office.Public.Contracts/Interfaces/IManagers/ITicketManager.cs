using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Dtos.Ticket;
using TD.Office.Public.Contracts.Requests.Ticket;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ITicketManager
{
	LSCoreSortedAndPagedResponse<TicketListDto> GetMultiple(GetMultipleTicketsRequest request);
	TicketDto GetSingle(LSCoreIdRequest request);
	List<TicketDto> GetRecentlySolved();
	List<TicketDto> GetInProgress();
	void Save(SaveTicketRequest request);
	void Delete(long id);
	void UpdatePriority(long id, UpdateTicketPriorityRequest request);
	void UpdateStatus(long id, UpdateTicketStatusRequest request);
	void UpdateDeveloperNotes(long id, UpdateDeveloperNotesRequest request);
}
