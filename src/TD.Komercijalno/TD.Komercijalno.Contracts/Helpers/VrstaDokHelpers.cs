using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class VrstaDokHelpers
    {
        public static ListResponse<VrstaDokDto> ToVrstaDokDtoListResponse(this List<VrstaDok> source)
        {
            var list = new List<VrstaDokDto>();

            foreach (var item in source)
                list.Add(item.ToVrstaDokDto());

            return new ListResponse<VrstaDokDto>(list);
        }

        public static VrstaDokDto ToVrstaDokDto(this VrstaDok item)
        {
            var dto = new VrstaDokDto();
            dto.InjectFrom(item);
            return dto;
        }
    }
}
