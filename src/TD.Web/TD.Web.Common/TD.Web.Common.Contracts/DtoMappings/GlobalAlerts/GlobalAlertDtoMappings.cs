using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Web.Common.Contracts.DtoMappings.GlobalAlerts;

public class GlobalAlertDtoMappings : ILSCoreDtoMapper<GlobalAlertEntity, GlobalAlertDto>
{
    public GlobalAlertDto ToDto(GlobalAlertEntity sender) =>
        new GlobalAlertDto
        {
            Text = sender.Text
        };
}