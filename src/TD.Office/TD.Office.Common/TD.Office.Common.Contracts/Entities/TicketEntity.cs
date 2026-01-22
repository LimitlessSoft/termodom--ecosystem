using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class TicketEntity : LSCoreEntity
{
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public TicketType Type { get; set; }
	public TicketStatus Status { get; set; }
	public TicketPriority Priority { get; set; }
	public long SubmittedByUserId { get; set; }
	public UserEntity? SubmittedByUser { get; set; }
	public string? DeveloperNotes { get; set; }
	public DateTime? ResolvedAt { get; set; }
	public long? ResolvedByUserId { get; set; }
	public UserEntity? ResolvedByUser { get; set; }
	public string? ResolutionNotes { get; set; }
}
