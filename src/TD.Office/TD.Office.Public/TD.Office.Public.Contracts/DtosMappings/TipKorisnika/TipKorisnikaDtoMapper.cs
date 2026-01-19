using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.TipKorisnika;

namespace TD.Office.Public.Contracts.DtosMappings.TipKorisnika;

public class TipKorisnikaDtoMapper : ILSCoreMapper<TipKorisnikaEntity, TipKorisnikaDto>
{
	public TipKorisnikaDto ToMapped(TipKorisnikaEntity sender)
	{
		var dto = new TipKorisnikaDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
