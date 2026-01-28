using LSCore.SortAndPage.Contracts;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;

namespace TD.Office.Public.Contracts.Requests.Ticket;

public class GetMultipleTicketsRequest : LSCoreSortableAndPageableRequest<TicketsSortColumnCodes.Tickets>
{
	public List<TicketType>? Types { get; set; }
	public List<TicketStatus>? Statuses { get; set; }
	public List<TicketPriority>? Priorities { get; set; }
	public long? SubmittedByUserId { get; set; }
}
