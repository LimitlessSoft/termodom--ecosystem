using LSCore.Contracts.Http;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class RobaHelpers
    {
        public static LSCoreListResponse<RobaDto> ToRobaDtoLSCoreListResponse(this List<Roba> source)
        {
            var list = new List<RobaDto>();
            foreach(var roba in source)
            {
                var robaDto = new RobaDto();
                robaDto.RobaId = roba.Id;
                robaDto.InjectFrom(roba);
                robaDto.Tarifa.InjectFrom(roba.Tarifa);
                list.Add(robaDto);
            }
            return new LSCoreListResponse<RobaDto>(list);
        }
    }
}
