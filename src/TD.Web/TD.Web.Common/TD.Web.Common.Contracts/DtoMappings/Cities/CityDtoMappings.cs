using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Entities;

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
