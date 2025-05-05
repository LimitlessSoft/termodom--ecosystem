using TD.Komercijalno.Contracts.Dtos.Promene;
using TD.Komercijalno.Contracts.Requests.Promene;

namespace TD.Komercijalno.Contracts.IManagers;

public interface IPromenaManager
{
	List<PromenaDto> GetMultiple(PromenaGetMultipleRequest request);
}
