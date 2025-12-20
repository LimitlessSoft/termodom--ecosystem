using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Dtos.Popisi;
using TD.Office.Public.Contracts.Requests.Popisi;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IPopisManager
{
	LSCoreSortedAndPagedResponse<PopisDto> GetMultiple(GetPopisiRequest request);
	Task<bool> CreateAsync(CreatePopisiRequest request);
	PopisDetailedDto GetById(long id);
	Task<bool> StornirajPopisAsync(long id);
	Task SetStatusAsync(PopisSetStatusRequest request);
	Task<PopisItemDto> AddItemToPopisAsync(PopisAddItemRequest request);
	Task RemoveItemFromPopisAsync(long id, long itemId);
	Task UpdatePopisanaKolicinaAsync(long id, long itemId, double popisanaKolicina);
	Task UpdateNarucenaKolicinaAsync(long id, long itemId, double narucenaKolicina);
	Task MasovnoDodavanjeStavkiAsync(long id, PopisMasovnoDodavanjeStavkiRequest request);
}
