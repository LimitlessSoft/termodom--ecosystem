using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.DtoMappings.NaciniPlacanja
{
    public static class NacinPlacanjaDtoMappings
    {
        public static List<NacinPlacanjaDto> ToNacinPlacanjaDtoList(this List<NacinPlacanja> naciniPlacanja)
        {
            var list = new List<NacinPlacanjaDto>();

            foreach(var np in naciniPlacanja)
            {
                var dto = new NacinPlacanjaDto();
                dto.InjectFrom(np);
                list.Add(dto);
            }
            return list;
        }
    }
}
