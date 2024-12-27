using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Notes;
public class CreateOrUpdateNoteRequest : LSCoreSaveRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
}
