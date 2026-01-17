using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.TipOdsustva;

namespace TD.Office.Public.Contracts.DtosMappings.TipOdsustva;

public class TipOdsustvaDtoMapper : ILSCoreMapper<TipOdsustvaEntity, TipOdsustvaDto>
{
	public TipOdsustvaDto ToMapped(TipOdsustvaEntity sender)
	{
		var dto = new TipOdsustvaDto();
		dto.InjectFrom(sender);
		return dto;
	}
}
