using LSCore.Mapper.Contracts;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Contracts.DtosMappings.SpecifikacijaNovca;

public class SpecifikacijaNovcaPutTempDtoMapping : ILSCoreMapper<SaveSpecifikacijaNovcaRequest, SpecifikacijaNovcaPutTempDto>
{
    public SpecifikacijaNovcaPutTempDto ToMapped(SaveSpecifikacijaNovcaRequest source)
    {
        return new SpecifikacijaNovcaPutTempDto
        {
            Komentar = source.Komentar,
            Eur1Komada = source.SpecifikacijaNovca.Eur1.Komada,
            Eur1Kurs = source.SpecifikacijaNovca.Eur1.Kurs,
            Eur2Komada = source.SpecifikacijaNovca.Eur2.Komada,
            Eur2Kurs = source.SpecifikacijaNovca.Eur2.Kurs,
            Novcanica5000Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 5000)?.Value ?? 0,
            Novcanica2000Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 2000)?.Value ?? 0,
            Novcanica1000Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 1000)?.Value ?? 0,
            Novcanica500Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 500)?.Value ?? 0,
            Novcanica200Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 200)?.Value ?? 0,
            Novcanica100Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 100)?.Value ?? 0,
            Novcanica50Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 50)?.Value ?? 0,
            Novcanica20Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 20)?.Value ?? 0,
            Novcanica10Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 10)?.Value ?? 0,
            Novcanica5Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 5)?.Value ?? 0,
            Novcanica2Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 2)?.Value ?? 0,
            Novcanica1Komada = source.SpecifikacijaNovca.Novcanice.FirstOrDefault(n => n.Key == 1)?.Value ?? 0,
            Kartice = source.SpecifikacijaNovca.Ostalo.First(x => x.Key == "Kartice").Vrednost,
            KarticeKomentar = source.SpecifikacijaNovca.Ostalo.FirstOrDefault(x => x.Key == "Kartice")?.Komentar,
            Cekovi = source.SpecifikacijaNovca.Ostalo.First(x => x.Key == "Cekovi").Vrednost,
            CekoviKomentar = source.SpecifikacijaNovca.Ostalo.FirstOrDefault(x => x.Key == "Cekovi")?.Komentar,
            Papiri = source.SpecifikacijaNovca.Ostalo.First(x => x.Key == "Papiri").Vrednost,
            PapiriKomentar = source.SpecifikacijaNovca.Ostalo.FirstOrDefault(x => x.Key == "Papiri")?.Komentar,
            Troskovi = source.SpecifikacijaNovca.Ostalo.First(x => x.Key == "Troskovi").Vrednost,
            TroskoviKomentar = source.SpecifikacijaNovca.Ostalo.FirstOrDefault(x => x.Key == "Troskovi")?.Komentar,
            Vozaci = source.SpecifikacijaNovca.Ostalo.First(x => x.Key == "Vozaci").Vrednost,
            VozaciKomentar = source.SpecifikacijaNovca.Ostalo.FirstOrDefault(x => x.Key == "Vozaci")?.Komentar,
            Sasa = source.SpecifikacijaNovca.Ostalo.First(x => x.Key == "Sasa").Vrednost,
            SasaKomentar = source.SpecifikacijaNovca.Ostalo.FirstOrDefault(x => x.Key == "Sasa")?.Komentar
        };
    }
}