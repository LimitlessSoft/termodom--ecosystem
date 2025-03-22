using LSCore.Mapper.Domain;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.GlobalAlerts;

namespace TD.Web.Common.Domain.Managers;

public class GlobalAlertManager(IGlobalAlertRepository repository) : IGlobalAlertManager
{
	public List<GlobalAlertDto> GetMultiple(GlobalAlertsGetMultipleRequest request) =>
		repository
			.GetMultiple()
			.Where(x => x.Application == request.Application)
			.ToMappedList<GlobalAlertEntity, GlobalAlertDto>();
}
