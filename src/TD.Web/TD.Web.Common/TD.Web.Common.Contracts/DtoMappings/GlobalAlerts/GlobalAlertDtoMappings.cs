using LSCore.Mapper.Contracts;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.GlobalAlerts;

public class GlobalAlertDtoMappings : ILSCoreMapper<GlobalAlertEntity, GlobalAlertDto>
{
	public GlobalAlertDto ToMapped(GlobalAlertEntity sender) =>
		new GlobalAlertDto { Text = sender.Text };
}
