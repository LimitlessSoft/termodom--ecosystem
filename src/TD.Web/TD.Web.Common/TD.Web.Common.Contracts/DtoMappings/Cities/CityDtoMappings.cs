using Omu.ValueInjecter;
using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Dtos.Cities;

namespace TD.Web.Common.Contracts.DtoMappings.Cities
{
    public class CityDtoMappings : ILSCoreDtoMapper<CityDto, CityEntity>
    {
        public CityDto ToDto(CityEntity sender)
        {
            var dto = new CityDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
