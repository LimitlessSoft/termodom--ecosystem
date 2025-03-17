using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;

namespace TD.Office.Public.Contracts.DtosMappings.KomercijalnoPrices;

public class KomercijalnoPriceGetDtoMapping
	: ILSCoreMapper<KomercijalnoPriceEntity, KomercijalnoPriceGetDto>
{
	public KomercijalnoPriceGetDto ToMapped(KomercijalnoPriceEntity sender) =>
		new()
		{
			RobaId = sender.RobaId,
			ProdajnaCenaBezPDV = sender.ProdajnaCenaBezPDV,
			NabavnaCenaBezPDV = sender.NabavnaCenaBezPDV
		};
}
