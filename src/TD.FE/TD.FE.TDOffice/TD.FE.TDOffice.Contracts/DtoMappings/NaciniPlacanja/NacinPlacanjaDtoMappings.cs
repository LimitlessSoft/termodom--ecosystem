using Omu.ValueInjecter;
using TD.FE.TDOffice.Contracts.Dtos.NaciniPlacanja;

namespace TD.FE.TDOffice.Contracts.DtoMappings.NaciniPlacanja
{
    public static class NacinPlacanjaDtoMappings
    {
        public static List<NacinPlacanjaDto> ToNacinPlacanjaDtoList(this List<Komercijalno.Contracts.Dtos.NaciniPlacanja.NacinPlacanjaDto> source)
        {
            var list = new List<NacinPlacanjaDto>();
            foreach(var item in source)
            {
                var dto = new NacinPlacanjaDto();
                dto.InjectFrom(item);
                list.Add(dto);
            }
            return list;
        }
    }
}
