using Omu.ValueInjecter;
using TD.Web.Contracts.Dtos.Units;
using TD.Web.Contracts.Entities;

namespace TD.Web.Contracts.DtoMappings.Units
{
    public static class UnitsGetDtoMappings
    {
        public static List<UnitsGetDto> ToListDto(this List<UnitEntity> sender)
        {
            var list = new List<UnitsGetDto>();
            foreach (var unit in sender)
                list.Add(unit.ToDto());
            return list;
        }
        public static UnitsGetDto ToDto(this UnitEntity sender)
        {
            var dto = new UnitsGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
