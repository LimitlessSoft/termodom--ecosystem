using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Ticket;

public class SaveTicketRequest
{
	public long? Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public TicketType Type { get; set; }
}
