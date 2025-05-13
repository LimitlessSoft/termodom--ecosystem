using TD.Komercijalno.Contracts.Dtos.IstorijaUplata;
using TD.Komercijalno.Contracts.Requests.IstorijaUplata;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IIstorijaUplataManager
{
	List<IstorijaUplataDto> GetMultiple(IstorijaUplataGetMultipleRequest request);
}
