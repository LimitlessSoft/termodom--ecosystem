using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Dtos.Popisi;
using TD.Office.Public.Contracts.Requests.Popisi;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IPopisManager
{
	LSCoreSortedAndPagedResponse<PopisDto> GetMultiple(GetPopisiRequest request);
	Task<bool> CreateAsync(CreatePopisiRequest request);
	PopisDetailedDto GetById(long id);
	bool StornirajPopis(long id);
	void SetStatus(PopisSetStatusRequest request);
	Task<PopisItemDto> AddItemToPopis(PopisAddItemRequest request);
	void RemoveItemFromPopis(long id, long itemId);
	Task UpdatePopisanaKolicinaAsync(long id, long itemId, double popisanaKolicina);
	void UpdateNarucenaKolicina(long id, long itemId, double narucenaKolicina);
}
