using Omu.ValueInjecter;
using Omu.ValueInjecter.Injections;
using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class DokumentiHelpers
    {
        public static ListResponse<DokumentDto> ToDokumentDtoListResponse(this List<Dokument> source)
        {
            var list = new List<DokumentDto>();

            foreach (var item in source)
                list.Add(item.ToDokumentDto());

            return new ListResponse<DokumentDto>(list);
        }

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
