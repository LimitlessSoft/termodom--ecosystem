using LSCore.Contracts;
using LSCore.Contracts.Requests;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Domain.Managers;
public class NotesManager(ILogger<NotesManager> logger, OfficeDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<NotesManager, NotesEntity>(logger, dbContext, contextUser), INotesManager
{
    public GetNoteDto GetSingle(LSCoreIdRequest id)
    {
        //TODO:
        throw new NotImplementedException();
    }

    public long Save(CreateOrUpdateNoteRequest request) =>
        Save(request, (entity) => entity.Id);
}
