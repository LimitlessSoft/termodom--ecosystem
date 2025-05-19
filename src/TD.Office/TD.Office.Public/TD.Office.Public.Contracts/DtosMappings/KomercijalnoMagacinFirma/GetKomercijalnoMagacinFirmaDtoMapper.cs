using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.KomercijalnoMagacinFirma;

namespace TD.Office.Public.Contracts.DtosMappings.KomercijalnoMagacinFirma;

public class GetKomercijalnoMagacinFirmaDtoMapper
	: ILSCoreMapper<KomercijalnoMagacinFirmaEntity, GetKomercijalnoMagacinFirmaDto>
{
	public GetKomercijalnoMagacinFirmaDto ToMapped(KomercijalnoMagacinFirmaEntity source)
	{
		var dto = new GetKomercijalnoMagacinFirmaDto();
		dto.InjectFrom(source);
		return dto;
	}
}
