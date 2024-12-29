using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Domain.Managers;
public class NoteManager(ILogger<NoteManager> logger,
        OfficeDbContext dbContext,
        LSCoreContextUser contextUser,
        INoteRepository noteRepository
    )
    : LSCoreManagerBase<NoteManager, NoteEntity>(logger, dbContext, contextUser), INoteManager
{
    public void DeleteNote(LSCoreIdRequest request)
    {
        if (noteRepository.GetById(request.Id).CreatedBy != contextUser.Id)
            throw new LSCoreForbiddenException();

        HardDelete(request.Id);
    }
        
    public GetNoteDto GetSingle(GetSingleNoteRequest request)
    {
        if (noteRepository.GetById(request.Id).CreatedBy != contextUser.Id)
            throw new LSCoreForbiddenException();
        return noteRepository.GetById(request.Id).ToDto<NoteEntity, GetNoteDto>();
    }

    public void RenameTab(RenameTabRequest request)
    {
        var entity = noteRepository.GetById(request.Id);
        
        if(entity.CreatedBy != contextUser.Id)
            throw new LSCoreForbiddenException();

        if (noteRepository.Exists((long)contextUser.Id, request.Name))
            throw new LSCoreBadRequestException(NotesValidationCodes.NVC_001.GetDescription()!);

        entity.Name = request.Name;
        Update(entity);
    }

    public long Save(CreateOrUpdateNoteRequest request)
    {
        if (request.IsOld && noteRepository.GetById(request.Id!.Value).CreatedBy != contextUser.Id)
            throw new LSCoreForbiddenException();

        if (noteRepository.Exists(contextUser.Id!.Value, request.Name, request.Id))
            throw new LSCoreBadRequestException(NotesValidationCodes.NVC_001.GetDescription()!);

        return Save(request, (entity) => entity.Id);
    }
}
