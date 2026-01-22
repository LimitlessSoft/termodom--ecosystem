using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Ticket;

namespace TD.Office.Public.Contracts.DtosMappings.Ticket;

public class TicketListDtoMapper : ILSCoreMapper<TicketEntity, TicketListDto>
{
	public TicketListDto ToMapped(TicketEntity sender)
	{
		var dto = new TicketListDto();
		dto.InjectFrom(sender);
		dto.SubmittedByUserNickname = sender.SubmittedByUser?.Nickname ?? string.Empty;
		return dto;
	}
}
