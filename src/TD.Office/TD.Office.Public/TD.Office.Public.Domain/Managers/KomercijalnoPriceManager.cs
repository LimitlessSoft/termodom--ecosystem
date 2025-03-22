using LSCore.Mapper.Domain;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.KomercijalnoPrices;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Interfaces.IRepositories;

namespace TD.Office.Public.Domain.Managers;

public class KomercijalnoPriceManager(IKomercijalnoPriceRepository komercijalnoPriceRepository)
	: IKomercijalnoPriceManager
{
	public List<KomercijalnoPriceGetDto> GetMultiple() =>
		komercijalnoPriceRepository
			.GetMultiple()
			.ToMappedList<KomercijalnoPriceEntity, KomercijalnoPriceGetDto>();
}
