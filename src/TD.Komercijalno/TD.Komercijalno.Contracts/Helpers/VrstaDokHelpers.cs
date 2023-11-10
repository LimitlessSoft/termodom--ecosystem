using LSCore.Contracts.Http;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class VrstaDokHelpers
    {
        public static LSCoreListResponse<VrstaDokDto> ToVrstaDokDtoLSCoreListResponse(this List<VrstaDok> source)
        {
            var list = new List<VrstaDokDto>();

            foreach (var item in source)
                list.Add(item.ToVrstaDokDto());

            return new LSCoreListResponse<VrstaDokDto>(list);
        }

        public static VrstaDokDto ToVrstaDokDto(this VrstaDok item)
        {
            var dto = new VrstaDokDto();
            dto.InjectFrom(item);
            return dto;
        }
    }
}
