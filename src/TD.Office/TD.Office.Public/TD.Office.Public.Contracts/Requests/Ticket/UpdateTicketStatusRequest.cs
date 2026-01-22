using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Ticket;

public class UpdateTicketStatusRequest
{
	public TicketStatus Status { get; set; }
	public string? ResolutionNotes { get; set; }
}
