using TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;
using TD.TDOffice.Contracts.Entities;
using Omu.ValueInjecter;

namespace TD.TDOffice.Contracts.DtoMappings.MCPartnerCenovnikItems
{
    public static class MCpartnerCenovnikItemEntityGetDtoMappings
    {
        public static List<MCpartnerCenovnikItemEntityGetDto> ToListDto(this List<MCPartnerCenovnikItemEntity> source)
        {
            var list = new List<MCpartnerCenovnikItemEntityGetDto>();

            foreach(var item in source)
                list.Add(item.ToDto());

            return list;
        }

        public static MCpartnerCenovnikItemEntityGetDto ToDto(this MCPartnerCenovnikItemEntity entity)
        {
            var dto = new MCpartnerCenovnikItemEntityGetDto();
            dto.InjectFrom(entity);
            dto.Id = entity.Id;
            return dto;
        }
    }
}
