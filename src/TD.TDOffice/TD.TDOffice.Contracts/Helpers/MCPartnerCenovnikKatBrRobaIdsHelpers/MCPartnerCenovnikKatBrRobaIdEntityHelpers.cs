using Omu.ValueInjecter;
using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikKatBrRobaIds;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Contracts.Helpers.MCPartnerCenovnikKatBrRobaIdsHelpers
{
    public static class MCPartnerCenovnikKatBrRobaIdEntityHelpers
    {
        public static List<MCPartnerCenovnikKatBrRobaIdGetDto> ToListDto(this List<MCPartnerCenovnikKatBrRobaIdEntity> source)
        {
            var list = new List<MCPartnerCenovnikKatBrRobaIdGetDto>();

            foreach(var entity in source)
                list.Add(entity.ToDto());

            return list;
        }

        public static MCPartnerCenovnikKatBrRobaIdGetDto ToDto(this MCPartnerCenovnikKatBrRobaIdEntity source)
        {
            var dto = new MCPartnerCenovnikKatBrRobaIdGetDto();
            dto.InjectFrom(source);
            return dto;
        }
    }
}
