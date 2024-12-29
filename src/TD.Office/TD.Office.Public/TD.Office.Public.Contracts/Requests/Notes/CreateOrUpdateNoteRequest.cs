using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Notes;
public class CreateOrUpdateNoteRequest : LSCoreSaveRequest
{
    public string Name { get; set; }
    public string Content { get; set; }
    public string? OldContent { get; set; }
}
