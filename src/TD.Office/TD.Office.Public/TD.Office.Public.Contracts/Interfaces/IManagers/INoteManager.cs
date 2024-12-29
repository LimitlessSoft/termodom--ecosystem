using LSCore.Contracts.Requests;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;
public interface INoteManager
{
    long Save(CreateOrUpdateNoteRequest request);
    GetNoteDto GetSingle(GetSingleNoteRequest request);
    void DeleteNote(LSCoreIdRequest request);
    void RenameTab(RenameTabRequest request);
    GetNotesDto GetNotes();
}
