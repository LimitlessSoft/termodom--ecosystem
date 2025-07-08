using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Enums.ValidationCodes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Domain.Managers;

public class NoteManager(
	LSCoreAuthContextEntity<string> contextEntity,
	INoteRepository noteRepository,
	IUserRepository userRepository
) : INoteManager
{
	public void DeleteNote(LSCoreIdRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (noteRepository.GetById(request.Id).CreatedBy != currentUser.Id)
			throw new LSCoreForbiddenException();

		if (!noteRepository.HasMoreThanOne(currentUser.Id))
			throw new LSCoreBadRequestException(NotesValidationCodes.NVC_002.GetDescription()!);

		noteRepository.HardDelete(request.Id);
	}

	public GetNoteDto GetSingle(GetSingleNoteRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();
		if (noteRepository.GetById(request.Id).CreatedBy != currentUser.Id)
			throw new LSCoreForbiddenException();

		var note = noteRepository.GetById(request.Id);
		currentUser.LastNoteId = request.Id;
		userRepository.Update(currentUser);
		return note.ToMapped<NoteEntity, GetNoteDto>();
	}

	public void RenameTab(RenameTabRequest request)
	{
		var entity = noteRepository.GetById(request.Id);
		var currentUser = userRepository.GetCurrentUser();
		if (entity.CreatedBy != currentUser.Id)
			throw new LSCoreForbiddenException();

		if (noteRepository.Exists(currentUser.Id, request.Name))
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
				new CreateOrUpdateNoteRequest() { Content = "", Name = $"{currentUser.Nickname}'s first note" }
			);
			userRepository.Update(currentUser);
		}

		var notesIdentifiers = noteRepository.GetNotesIdentifiers(currentUser.Id);

		return new GetNotesDto
		{
			LastNoteId = currentUser.LastNoteId!.Value,
			Notes = notesIdentifiers
		};
	}

	public long Save(CreateOrUpdateNoteRequest request)
	{
		var currentUser = userRepository.GetCurrentUser();

		if (
			request.Id.HasValue
			&& noteRepository.GetById(request.Id!.Value).CreatedBy != currentUser.Id
		)
			throw new LSCoreForbiddenException();

		if (noteRepository.Exists(currentUser.Id, request.Name, request.Id))
			throw new LSCoreBadRequestException(NotesValidationCodes.NVC_001.GetDescription()!);

		if (request.Id.HasValue)
		{
			var oldNote = noteRepository.GetById(request.Id!.Value);
			if (oldNote.Content != request.OldContent)
				throw new LSCoreBadRequestException(NotesValidationCodes.NVC_003.GetDescription()!);
		}

		var entity = request.Id.HasValue
			? noteRepository.GetById(request.Id.Value)
			: new NoteEntity();

		entity.InjectFrom(request);
		entity.CreatedBy = currentUser.Id;
		noteRepository.UpdateOrCreate(entity);
		return entity.Id;
	}
}
