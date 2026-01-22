using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Dtos.Ticket;

public class TicketListDto
{
	public long Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public TicketType Type { get; set; }
	public TicketStatus Status { get; set; }
	public TicketPriority Priority { get; set; }
	public string SubmittedByUserNickname { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; }
}
