using Omu.ValueInjecter;
using TD.Core.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.Units;
using TD.Web.Admin.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Units
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
