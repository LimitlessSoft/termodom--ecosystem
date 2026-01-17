using TD.Office.Public.Contracts.Dtos.TipOdsustva;
using TD.Office.Public.Contracts.Requests.TipOdsustva;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ITipOdsustvaManager
{
	List<TipOdsustvaDto> GetMultiple();
	void Save(SaveTipOdsustvaRequest request);
	void Delete(long id);
}
