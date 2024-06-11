using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Entities;
using Omu.ValueInjecter;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class RobaHelpers
    {
        public static List<RobaDto> ToRobaDtoList(this List<Roba> source)
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

            return list;
        }
    }
}
