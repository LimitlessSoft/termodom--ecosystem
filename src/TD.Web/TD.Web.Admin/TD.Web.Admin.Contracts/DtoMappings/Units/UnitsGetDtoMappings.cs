using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Web.Admin.Contracts.DtoMappings.Units;

public class UnitsGetDtoMappings : ILSCoreDtoMapper<UnitEntity, UnitsGetDto>
{
    public UnitsGetDto ToDto(UnitEntity sender)
    {
        var dto = new UnitsGetDto();
        dto.InjectFrom(sender);
        return dto;
    }
}