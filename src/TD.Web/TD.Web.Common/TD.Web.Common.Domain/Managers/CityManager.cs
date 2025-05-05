using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.Cities;

namespace TD.Web.Common.Domain.Managers;

public class CityManager(ICityRepository cityRepository) : ICityManager
{
	public List<CityDto> GetMultiple(GetMultipleCitiesRequest request) =>
		cityRepository
			.GetMultiple()
			.Where(x => x.IsActive)
			.SortQuery(request, CitiesSortColumnCodes.CitiesSortRules)
			.ToMappedList<CityEntity, CityDto>();
}
