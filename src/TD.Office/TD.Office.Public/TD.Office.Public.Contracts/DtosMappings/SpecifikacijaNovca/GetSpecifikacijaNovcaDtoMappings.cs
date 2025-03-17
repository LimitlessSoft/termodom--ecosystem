using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;

namespace TD.Office.Public.Contracts.DtosMappings.SpecifikacijaNovca;

public class GetSpecifikacijaNovcaDtoMappings
	: ILSCoreMapper<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>
{
	public GetSpecifikacijaNovcaDto ToMapped(SpecifikacijaNovcaEntity sender)
	{
		var dto = new GetSpecifikacijaNovcaDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
