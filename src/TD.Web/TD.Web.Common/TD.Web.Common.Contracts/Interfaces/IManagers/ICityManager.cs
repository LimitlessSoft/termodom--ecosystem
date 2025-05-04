using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Requests.Cities;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface ICityManager
{
	List<CityDto> GetMultiple(GetMultipleCitiesRequest request);
}
