using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;

namespace TD.Office.Public.Contracts.DtosMappings.NalogZaPrevoz;

public class GetNalogZaPrevozDtoMapper : ILSCoreMapper<NalogZaPrevozEntity, GetNalogZaPrevozDto>
{
	public GetNalogZaPrevozDto ToMapped(NalogZaPrevozEntity sender)
	{
		var dto = new GetNalogZaPrevozDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
