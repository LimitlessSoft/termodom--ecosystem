using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Notes;

namespace TD.Office.Public.Contracts.DtosMappings.Notes;

public class GetNoteDtoMapper : ILSCoreMapper<NoteEntity, GetNoteDto>
{
	public GetNoteDto ToMapped(NoteEntity sender)
	{
		var dto = new GetNoteDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
