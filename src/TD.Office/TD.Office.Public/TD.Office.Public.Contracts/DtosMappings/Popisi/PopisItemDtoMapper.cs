using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Popisi;

namespace TD.Office.Public.Contracts.DtosMappings.Popisi;

public class PopisItemDtoMapper : ILSCoreMapper<PopisItemEntity, PopisItemDto>
{
	public PopisItemDto ToMapped(PopisItemEntity source)
	{
		return ToMappedStatic(source);
	}

	public static PopisItemDto ToMappedStatic(PopisItemEntity source)
	{
		return new PopisItemDto
		{
			Id = source.Id,
			PopisanaKolicina = source.PopisanaKolicina ?? 0,
			NarucenaKolicina = source.NarucenaKolicina,
			RobaId = source.RobaId,
		};
	}
}
