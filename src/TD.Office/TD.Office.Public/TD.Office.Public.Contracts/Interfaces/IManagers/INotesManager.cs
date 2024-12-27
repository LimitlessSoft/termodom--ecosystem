using LSCore.Contracts.Requests;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;
public interface INotesManager
{
    long Save(CreateOrUpdateNoteRequest request);
    GetNoteDto GetSingle(LSCoreIdRequest id);
}
