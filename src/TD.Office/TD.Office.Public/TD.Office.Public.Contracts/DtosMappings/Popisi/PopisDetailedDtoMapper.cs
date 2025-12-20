using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Popisi;

namespace TD.Office.Public.Contracts.DtosMappings.Popisi;

public class PopisDetailedDtoMapper : ILSCoreMapper<PopisDokumentEntity, PopisDetailedDto>
{
	public PopisDetailedDto ToMapped(PopisDokumentEntity source)
	{
		return new PopisDetailedDto
		{
			Id = source.Id,
			Date = source.CreatedAt,
			Type = source.Type,
			Status = source.Status,
			Items = source.Items?.Select(PopisItemDtoMapper.ToMappedStatic).ToList() ?? [],
			KomercijalnoPopisBrDok = (int)source.KomercijalnoPopisBrDok,
			KomercijalnoNarudzbenicaBrDok = (int?)source.KomercijalnoNarudzbenicaBrDok,
			UserName = source.User?.Nickname ?? string.Empty,
		};
	}
}
