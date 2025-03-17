namespace TD.Office.Public.Contracts.Requests.Notes;

public class CreateOrUpdateNoteRequest
{
	public long? Id { get; set; }
	public string Name { get; set; }
	public string Content { get; set; }
	public string? OldContent { get; set; }
}
