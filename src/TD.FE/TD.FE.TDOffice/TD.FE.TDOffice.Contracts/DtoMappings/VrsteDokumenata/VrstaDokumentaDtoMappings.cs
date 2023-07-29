using System.ComponentModel;
using TD.FE.TDOffice.Contracts.Dtos.VrsteDokumenata;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.FE.TDOffice.Contracts.DtoMappings.VrsteDokumenata
{
    public static class VrstaDokumentaDtoMappings
    {
        public static List<VrstaDokumentaDto> ToVrstaDokumentaDtoList(this List<VrstaDokDto> vrstaDoks)
        {
            var list = new List<VrstaDokumentaDto>();
            foreach(var vrDok in vrstaDoks)
            {
                var dto = new VrstaDokumentaDto();
                dto.Id = vrDok.VrDok;
                dto.Naziv = vrDok.NazivDok;
                list.Add(dto);
            }
            return list;
        }
    }
}
