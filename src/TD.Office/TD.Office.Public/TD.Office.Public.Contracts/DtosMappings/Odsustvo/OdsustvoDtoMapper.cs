using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Odsustvo;

namespace TD.Office.Public.Contracts.DtosMappings.Odsustvo;

public class OdsustvoDtoMapper : ILSCoreMapper<OdsustvoEntity, OdsustvoDto>
{
	public OdsustvoDto ToMapped(OdsustvoEntity sender)
	{
		var dto = new OdsustvoDto();
		dto.InjectFrom(sender);
		dto.TipOdsustvaNaziv = sender.TipOdsustva?.Naziv ?? string.Empty;
		return dto;
	}
}
