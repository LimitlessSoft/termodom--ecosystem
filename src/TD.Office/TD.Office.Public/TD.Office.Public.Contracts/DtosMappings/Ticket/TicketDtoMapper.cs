using LSCore.Mapper.Contracts;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Ticket;

namespace TD.Office.Public.Contracts.DtosMappings.Ticket;

public class TicketDtoMapper : ILSCoreMapper<TicketEntity, TicketDto>
{
	public TicketDto ToMapped(TicketEntity sender)
	{
		var dto = new TicketDto();
		dto.InjectFrom(sender);
		dto.SubmittedByUserNickname = sender.SubmittedByUser?.Nickname ?? string.Empty;
		dto.ResolvedByUserNickname = sender.ResolvedByUser?.Nickname ?? string.Empty;
		return dto;
	}
}
