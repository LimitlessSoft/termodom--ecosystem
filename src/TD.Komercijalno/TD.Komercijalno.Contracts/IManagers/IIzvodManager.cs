using TD.Komercijalno.Contracts.Requests.Izvodi;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IIzvodManager
{
	List<IzvodDto> GetMultiple(IzvodGetMultipleRequest request);
}
