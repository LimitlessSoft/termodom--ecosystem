using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Popisi;

namespace TD.Office.Public.Contracts.DtosMappings.Popisi;

public class PopisDtoMapper : ILSCoreMapper<PopisDokumentEntity, PopisDto>
{
	public PopisDto ToMapped(PopisDokumentEntity source)
	{
		return new PopisDto
		{
			Id = source.Id,
			Magacin = source.MagacinId.ToString(),
			Datum = source.CreatedAt.Date,
			BrojDokumenta = source.Id.ToString(),
			Status = source.Status,
		};
	}
}
