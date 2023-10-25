using Omu.ValueInjecter;
using TD.Core.Contracts.Interfaces;
using TD.Web.Contracts.Dtos.Units;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.Units
{
    public class UnitsGetDtoMappings : IDtoMapper<UnitsGetDto, UnitEntity>
    {
        public List<UnitsGetDto> ToListDto(List<UnitEntity> sender)
        {
            var list = new List<UnitsGetDto>();
            foreach (var unit in sender)
                list.Add(ToDto(unit));
            return list;
        }

        public UnitsGetDto ToDto(UnitEntity sender)
        {
            var dto = new UnitsGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
