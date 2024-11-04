using System.ComponentModel;
using TD.Office.Common.Contracts.Attributes;

namespace TD.Office.Common.Contracts.Enums;

public enum Permission
{
    [Description("Pristup aplikaciji")]
    Access,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.NalogZaPrevoz)]
    [Description("Nalog za prevoz - pregled")]
    NalogZaPrevozRead,

    [PermissionGroup(Constants.PermissionGroup.NalogZaPrevoz)]
    [Description("Nalog za prevoz - rad sa svim magacinima")]
    NalogZaPrevozRadSaSvimMagacinima,

    [PermissionGroup(Constants.PermissionGroup.NalogZaPrevoz)]
    [Description("Nalog za prevoz - novi")]
    NalogZaPrevozNovi,

    [PermissionGroup(Constants.PermissionGroup.NalogZaPrevoz)]
    [Description("Nalog za prevoz - prethodni datumi")]
    NalogZaPrevozPrethodniDatumi,

    [PermissionGroup(Constants.PermissionGroup.NalogZaPrevoz)]
    [Description("Nalog za prevoz - stampa izvestaja")]
    NalogZaPrevozStampaIzvestaja,

    [PermissionGroup(Constants.PermissionGroup.NalogZaPrevoz)]
    [Description("Nalog za prevoz - stampa pojedinacnog naloga")]
    NalogZaPrevozStampaPojedinacnogNaloga,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.Korisnici)]
    [Description("Korisnici - pregled")]
    KorisniciRead,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.Web)]
    [Description("Web - pregled")]
    WebRead,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.Partneri)]
    [Description("Partneri - pregled")]
    PartneriRead,

    [PermissionGroup(Constants.PermissionGroup.Partneri)]
    [Description("Partneri - vidi mobilni")]
    PartneriVidiMobilni,

    [PermissionGroup(Constants.PermissionGroup.Partneri)]
    [Description("Partneri - skoro kreirani")]
    PartneriSkoroKreirani,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.SpecifikacijaNovca)]
    [Description("Specifikacija Novca - pregled")]
    SpecifikacijaNovcaRead,

    [PermissionGroup(Constants.PermissionGroup.SpecifikacijaNovca)]
    [Description("Specifikacija Novca - pregled svih magacina")]
    SpecifikacijaNovcaSviMagacini,

    [PermissionGroup(Constants.PermissionGroup.SpecifikacijaNovca)]
    [Description("Specifikacija Novca - pregled prethodnih 7 dana")]
    SpecifikacijaNovcaPrethodnih7Dana,

    [PermissionGroup(Constants.PermissionGroup.SpecifikacijaNovca)]
    [Description("Specifikacija Novca - pregled svih datuma")]
    SpecifikacijaNovcaSviDatumi,

    [PermissionGroup(Constants.PermissionGroup.SpecifikacijaNovca)]
    [Description("Specifikacija Novca - pretraga po broju")]
    SpecifikacijaNovcaPretragaPoBroju,

    [PermissionGroup(Constants.PermissionGroup.SpecifikacijaNovca)]
    [Description("Specifikacija Novca - rad sa svim magacinima")]
    SpecifikacijaNovcaSave,

    [PermissionGroup(Constants.PermissionGroup.SpecifikacijaNovca)]
    [Description("Specifikacija Novca - stampa izvestaja")]
    SpecifikacijaNovcaPrint,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.PartnerIzvestajFinansijskoKomercijalno)]
    [Description(
        "Partneri - izvestaj stanja po godinama finansijsko i komercijalno - pristup modulu"
    )]
    PartneriKomercijalnoFinansijskoPoGodinamaRead,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.IzvestajUkupneKolicinePoRobiUFiltriranimDokumentima)]
    [Description("Izvestaji - Izveštaj ukupne količine u dokumentima po robi")]
    IzvestajUkupneKolicinePoRobiUFiltriranimDokumentimaRead,

    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.Proracuni)]
    [Description("Proracuni - pristup modulu")]
    ProracuniRead,
}
