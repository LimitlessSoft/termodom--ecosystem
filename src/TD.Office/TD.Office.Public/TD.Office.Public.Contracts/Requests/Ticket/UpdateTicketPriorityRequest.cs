using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Ticket;

public class UpdateTicketPriorityRequest
{
	public TicketPriority Priority { get; set; }
}
