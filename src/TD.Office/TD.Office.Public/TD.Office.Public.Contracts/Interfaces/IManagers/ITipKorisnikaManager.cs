using TD.Office.Public.Contracts.Dtos.TipKorisnika;
using TD.Office.Public.Contracts.Requests.TipKorisnika;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ITipKorisnikaManager
{
	List<TipKorisnikaDto> GetMultiple();
	void Save(SaveTipKorisnikaRequest request);
	void Delete(long id);
}
