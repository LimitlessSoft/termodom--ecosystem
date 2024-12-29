using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Notes;

namespace TD.Office.Public.Contracts.DtosMappings.Notes;
public class GetNoteDtoMapper : ILSCoreDtoMapper<NoteEntity, GetNoteDto>
{
    public GetNoteDto ToDto(NoteEntity sender)
    {
        var dto = new GetNoteDto();
        dto.InjectFrom(sender);
        return dto;
    }
}
