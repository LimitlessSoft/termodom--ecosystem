using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class RobaHelpers
    {
        public static ListResponse<RobaDto> ToRobaDtoListResponse(this List<Roba> source)
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
            return new ListResponse<RobaDto>(list);
        }
    }
}
