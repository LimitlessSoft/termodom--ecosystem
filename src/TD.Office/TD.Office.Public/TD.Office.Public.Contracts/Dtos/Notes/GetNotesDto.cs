namespace TD.Office.Public.Contracts.Dtos.Notes;

public class GetNotesDto
{
    public long LastNoteId { get; set; }
    public Dictionary<long, string> Notes { get; set; } = new ();
}