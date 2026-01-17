using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Odsustvo;

namespace TD.Office.Public.Contracts.DtosMappings.Odsustvo;

public class OdsustvoCalendarDtoMapper : ILSCoreMapper<OdsustvoEntity, OdsustvoCalendarDto>
{
	public OdsustvoCalendarDto ToMapped(OdsustvoEntity sender)
	{
		var dto = new OdsustvoCalendarDto();
		dto.InjectFrom(sender);
		dto.UserNickname = sender.User?.Nickname ?? string.Empty;
		dto.TipOdsustvaNaziv = sender.TipOdsustva?.Naziv ?? string.Empty;
		return dto;
	}
}
