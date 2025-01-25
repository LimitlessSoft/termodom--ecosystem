using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Domain.Managers;

public class NoteManager(
    ILogger<NoteManager> logger,
    LSCoreContextUser contextUser,
    INoteRepository noteRepository,
    IUserRepository userRepository
) : INoteManager
{
    public void DeleteNote(LSCoreIdRequest request)
    {
        if (noteRepository.GetById(request.Id).CreatedBy != contextUser.Id)
            throw new LSCoreForbiddenException();

        if (!noteRepository.HasMoreThanOne(contextUser.Id!.Value))
            throw new LSCoreBadRequestException(NotesValidationCodes.NVC_002.GetDescription()!);

        noteRepository.HardDelete(request.Id);
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

        if (entity.CreatedBy != contextUser.Id)
            throw new LSCoreForbiddenException();

        if (noteRepository.Exists((long)contextUser.Id, request.Name))
            throw new LSCoreBadRequestException(NotesValidationCodes.NVC_001.GetDescription()!);

        entity.Name = request.Name;
        noteRepository.Update(entity);
    }

    public GetNotesDto GetNotes()
    {
        var currentUser = userRepository.GetCurrentUser();

        if (currentUser.LastNoteId is null or 0)
        {
            currentUser.LastNoteId = Save(
                new CreateOrUpdateNoteRequest() { Content = "", Name = "First note" }
            );
            userRepository.Update(currentUser);
        }

        var notesIdentifiers = noteRepository.GetNotesIdentifiers(contextUser.Id!.Value);

        return new GetNotesDto
        {
            LastNoteId = currentUser.LastNoteId!.Value,
            Notes = notesIdentifiers
        };
    }

    public long Save(CreateOrUpdateNoteRequest request)
    {
        if (request.IsOld && noteRepository.GetById(request.Id!.Value).CreatedBy != contextUser.Id)
            throw new LSCoreForbiddenException();

        if (noteRepository.Exists(contextUser.Id!.Value, request.Name, request.Id))
            throw new LSCoreBadRequestException(NotesValidationCodes.NVC_001.GetDescription()!);

        if (request.IsOld)
        {
            var oldNote = noteRepository.GetById(request.Id!.Value);
            if (oldNote.Content != request.OldContent)
                throw new LSCoreBadRequestException(NotesValidationCodes.NVC_003.GetDescription()!);
        }

        var entity = request.Id.HasValue
            ? noteRepository.GetById(request.Id.Value)
            : new NoteEntity();

        entity.InjectFrom(request);
        noteRepository.UpdateOrCreate(entity);
        return entity.Id;
    }
}
