using LSCore.Contracts.Http;
using Omu.ValueInjecter;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Helpers
{
    public static class StavkeHelpers
    {
        public static LSCoreListResponse<StavkaDto> ToStavkaDtoLSCoreListResponse(this List<Stavka> source)
        {
            return new LSCoreListResponse<StavkaDto>(source.ToStavkaDtoList());
        }

        public static List<StavkaDto> ToStavkaDtoList(this List<Stavka> source)
        {
            var list = new List<StavkaDto>();

            foreach (var item in source)
                list.Add(item.ToStavkaDto());

            return list;
        }

        public static StavkaDto ToStavkaDto(this Stavka item)
        {
            var dto = new StavkaDto();
            dto.InjectFrom(item);

            //if(item.Magacin != null)
            //{
            //    dto.Magacin = new MagacinDto();
            //    dto.Magacin.InjectFrom(item.Magacin);
            //}

            //if (item.Dokument != null)
            //{
            //    dto.Dokument = new DokumentDto();
            //    dto.Dokument.InjectFrom(item.Dokument);
            //}

            return dto;
        }
    }
}
