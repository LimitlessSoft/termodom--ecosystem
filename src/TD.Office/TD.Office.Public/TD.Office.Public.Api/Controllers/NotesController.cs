using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access)]
public class NotesController(INoteManager notesManager) : ControllerBase
{
	[HttpGet]
	[Route("/notes")]
	public GetNotesDto GetNotes() => notesManager.GetNotes();

	[HttpPut]
	[Route("/notes")]
	public long CreateOrUpdateNote([FromBody] CreateOrUpdateNoteRequest request) =>
		notesManager.Save(request);

	[HttpGet]
	[Route("/notes/{Id}")]
	public GetNoteDto GetSingle([FromRoute] GetSingleNoteRequest request) =>
		notesManager.GetSingle(request);

	[HttpDelete]
	[Route("/notes/{Id}")]
	public void DeleteNote([FromRoute] LSCoreIdRequest request) => notesManager.DeleteNote(request);

	[HttpPut]
	[Route("/notes/{id}/name")]
	public void RenameTab([FromRoute] long id, RenameTabRequest request)
	{
		request.Id = id;
		notesManager.RenameTab(request);
	}
}
