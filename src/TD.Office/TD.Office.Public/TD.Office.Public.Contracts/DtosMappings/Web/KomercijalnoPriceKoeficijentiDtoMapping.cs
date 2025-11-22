using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Web;

namespace TD.Office.Public.Contracts.DtosMappings.Web;

public class KomercijalnoPriceKoeficijentiDtoMapping
	: ILSCoreMapper<KomercijalnoPriceKoeficijentEntity, KomercijalnoPriceKoeficijentiDto.Item>
{
	public KomercijalnoPriceKoeficijentiDto.Item ToMapped(KomercijalnoPriceKoeficijentEntity source)
	{
		return new KomercijalnoPriceKoeficijentiDto.Item
		{
			Id = source.Id,
			Naziv = source.Naziv,
			Vrednost = source.Vrednost,
		};
	}
}
