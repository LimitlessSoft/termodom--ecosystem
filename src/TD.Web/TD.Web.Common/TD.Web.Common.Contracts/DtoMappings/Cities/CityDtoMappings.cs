using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Web.Common.Contracts.DtoMappings.Cities;

public class CityDtoMappings : ILSCoreDtoMapper<CityEntity, CityDto>
{
    public CityDto ToDto(CityEntity sender)
    {
        var dto = new CityDto();
        dto.InjectFrom(sender);
        return dto;
    }
}