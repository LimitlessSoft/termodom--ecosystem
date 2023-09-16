using TD.FE.TDOffice.Contracts.Dtos.WebUredjivanjeProizvoda;
using TD.Komercijalno.Contracts.Dtos.Roba;

namespace TD.FE.TDOffice.Contracts.DtoMappings
{
    public static class WebUredjivanjeProizvodaProizvodiGetDtoMappings
    {
        public static List<WebUredjivanjeProizvodaProizvodiGetDto> ConvertToWebUredjivanjeProizvodaProizvodiGetDtoList(
            List<ProductsGetDto> webProizvodi, List<RobaDto> komercijalnoRoba)
        {
            var dto = new List<WebUredjivanjeProizvodaProizvodiGetDto>();

            foreach(var wp in webProizvodi)
            {
                var kr = komercijalnoRoba.FirstOrDefault(x => x.RobaId == wp.Id);

                dto.Add(new WebUredjivanjeProizvodaProizvodiGetDto()
                {
                    RobaId = wp.Id,
                    WebJm = wp.Unit,
                    WebKatBr = wp.SKU,
                    WebNaziv = wp.Name,
                    KomercijalnoJm = kr == null ? "Nije pronadjen proizvod" : kr.JM,
                    KomercijalnoKatBr = kr == null ? "Nije pronadjen proizvod" : kr.KatBr,
                    KomercijalnoNaziv = kr == null ? "Nije pronadjen proizvod" : kr.Naziv
                });
            }

            return dto;
        }
    }
}
