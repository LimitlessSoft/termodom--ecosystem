using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Dtos.GlobalAlerts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.GlobalAlerts
{
    public class GlobalAlertDtoMappings : ILSCoreDtoMapper<GlobalAlertDto, GlobalAlertEntity>
    {
        public GlobalAlertDto ToDto(GlobalAlertEntity sender) =>
            new GlobalAlertDto
            {
                Text = sender.Text
            };
    }
}
