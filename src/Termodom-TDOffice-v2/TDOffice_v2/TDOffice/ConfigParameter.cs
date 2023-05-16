using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public enum ConfigParameter
    {
        Undefined0 = 0,
        Undefined1 = 1,
        KoeficijentMinZalihe = 7,
        KoeficijentPrekomernihZaliha = 8,
        ListaIDevaPrevoznika = 154,
        ListaOkupiranihKupaca_MojKupac = 155, // Dictionary<int, int> <PPID, korisnikID>
        BelekseKupaca_MojKupac = 156, // Dictionary<int ,string> <PPID, string>
        MinimalnaVerzija = 1337,
        ConnectionStringsKomercijalno = 1338, // Dictionary<int, string> <godina, string>
        ConnectionStringConfig = 1340, // string
        NalogZaIspravkuPoslednjiBroj = 2000, // Skladisti poslednji broj naloga za ispravku // Dictionary<int, Dictionary<int, int>> magacin brMpRacuna dodeljeniBroj
        OsvezavanjePrava = 4276,
        PatchLog = 4326, // Dictionary<DateTime, string> // Datum - tekst
        MaksimalnaZastarelostProracunaPrilikomPrebacivanjaUMPRacun = 5283,
        BiznisPlan = 11002, // Dictionary<int, BiznisPlan> // magacinID, BiznisPlan
        Lager0634 = 11475, // Dictionary<int, double> // Robaid, kolicina
        Lager0634RazduzenaDokumenta = 11476, // List<int>
        SvedenoStanje1934 = 11477, // List<int> // List<brDok34>
        GrupeTroskova = 11580, // Dictionary<string, List<int>> // imegrupe, lista<RobaiD>(usluga)
        TekuciRacunZaCekove = 11582, // Tekuci racun koji se koristi za cekove a vezan je za Komercijalno.Banka // Dictionary<int, string> // Banka.ID, tekuciRacun
        KomercijalnoParametriSablon = 5749700, // Dictionary<string, string> // KomercijalnoParametarNaziv, SablonskaVrednostParametra
        Pokloni = 5749782,
        MagacinUPopisu = 25092021,
        IspravnostIzvodStavkeTaskConfig = 25092022
    }
}
