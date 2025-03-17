using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Proracuni;

namespace TD.Office.Public.Contracts.DtosMappings.Proracuni;

public class ProracunItemDtoMapping : ILSCoreMapper<ProracunItemEntity, ProracunItemDto>
{
	public ProracunItemDto ToMapped(ProracunItemEntity sender) =>
		new()
		{
			Id = sender.Id,
			RobaId = sender.RobaId,
			Kolicina = sender.Kolicina,
			CenaBezPdv = sender.CenaBezPdv,
			Pdv = sender.Pdv,
			Rabat = sender.Rabat
		};
}
