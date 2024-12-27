using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.Notes;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Notes;

namespace TD.Office.Public.Api.Controllers;

[Authorize]
[ApiController]
[Permissions(Permission.Access)]
public class NotesController(INotesManager notesManager)
    : ControllerBase
{
    [HttpPut]
    [Route("/notes")]
    public long CreateOrUpdateNote([FromBody]CreateOrUpdateNoteRequest request) =>
        notesManager.Save(request);

    [HttpGet]
    [Route("/notes/{id}")]
    public GetNoteDto GetSingle([FromRoute] LSCoreIdRequest id) =>
        notesManager.GetSingle(id);

}
