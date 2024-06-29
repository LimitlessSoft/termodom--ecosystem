using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;
using Omu.ValueInjecter;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class DokumentiHelpers
    {
        public static List<DokumentDto> ToDokumentListDto(this List<Dokument> source) =>
            source.Select(item => item.ToDokumentDto()).ToList();

        public static DokumentDto ToDokumentDto(this Dokument item)
        {
            var dto = new DokumentDto();
            dto.InjectFrom(item);

            if (item.VrstaDok != null)
            {
                dto.VrstaDok = new Dtos.VrstaDok.VrstaDokDto();
                dto.VrstaDok.InjectFrom(item.VrstaDok);
            }

            if(item.Stavke != null && item.Stavke.Count > 0)
            {
                dto.Stavke = new List<StavkaDto>();
                item.Stavke.ToStavkaDtoList().ForEach(x =>
                {
                    dto.Stavke.Add(x);
                });
            }

            return dto;
        }
    }
}
