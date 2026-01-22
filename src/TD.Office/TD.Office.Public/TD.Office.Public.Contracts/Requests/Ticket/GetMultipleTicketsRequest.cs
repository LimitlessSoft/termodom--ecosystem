using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Ticket;

public class GetMultipleTicketsRequest
{
	public TicketType? Type { get; set; }
	public TicketStatus? Status { get; set; }
	public TicketPriority? Priority { get; set; }
	public long? SubmittedByUserId { get; set; }
}
