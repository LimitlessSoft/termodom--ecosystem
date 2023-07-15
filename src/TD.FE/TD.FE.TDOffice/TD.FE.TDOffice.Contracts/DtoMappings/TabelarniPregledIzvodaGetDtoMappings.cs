using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.TabelarniPregledIzvoda;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.TDOffice.Contracts.Entities;

namespace TD.FE.TDOffice.Contracts.DtoMappings
{
    public static class TabelarniPregledIzvodaGetDtoMappings
    {
        public static List<TabelarniPregledIzvodaGetDto> ConvertToTabelarniPregledIzvodaGetDtoList(List<DokumentDto> dokumentDtos, List<DokumentTagIzvod> dokumentTagizvodDtos)
        {
            var list = new List<TabelarniPregledIzvodaGetDto>();

            foreach (var dokument in dokumentDtos)
            {
                var dokumentTagDto = dokumentTagizvodDtos.FirstOrDefault(x => x.BrojDokumentaIzvoda == dokument.BrDok);

                list.Add(new TabelarniPregledIzvodaGetDto()
                {
                    BrDok = dokument.BrDok,
                    DatumIzvoda = dokument.Datum,
                    UnosDuguje = dokumentTagDto == null ? 0 : dokumentTagDto.UnosDuguje,
                    UnosPocetnoStanje = dokumentTagDto == null ? 0 : dokumentTagDto.UnosPocetnoStanje,
                    UnosPotrazuje = dokumentTagDto == null ? 0 : dokumentTagDto.UnosPotrazuje,
                    FinansijskaIspravnost = false,
                    FirmaPib = "todo",
                    IntBroj = dokument.IntBroj,
                    Korisnik = dokumentTagDto == null ? 0 : dokumentTagDto.Korisnik,
                    LogickaIspravnost = false,
                    VrDok = dokument.VrDok,
                    Zakljucano = dokument.Flag == 1
                });
            }

            return list;
        }
    }
}
