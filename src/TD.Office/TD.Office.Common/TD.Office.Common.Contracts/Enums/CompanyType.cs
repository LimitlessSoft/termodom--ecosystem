using System.ComponentModel;

namespace TD.Office.Common.Contracts.Enums;
public enum CompanyType
{
    [Description("Društvo sa ograničenom odgovornošću")]
    DOO,  // Društvo sa ograničenom odgovornošću
    [Description("Akcionarsko društvo")]
    AD,   // Akcionarsko društvo
    [Description("Preduzetnik")]
    PR,   // Preduzetnik
    [Description("Ortačko društvo")]
    OD,   // Ortačko društvo
    [Description("Komanditno društvo")]
    KD,   // Komanditno društvo
    [Description("Samostalna zanatska radnja")]
    SZR,  // Samostalna zanatska radnja
    [Description("Samostalna trgovinska radnja")]
    STR,  // Samostalna trgovinska radnja
    [Description("Fondacija")]
    F,    // Fondacija
    [Description("Udruženje građana")]
    UG,   // Udruženje građana
    [Description("Zadruga")]
    Z,    // Zadruga
    [Description("Predstavništvo")]
    PT,   // Predstavništvo
    [Description("Fizičko lice")]
    FizickoLice,   // Fizičko lice
    [Description("Budžetni korisnik")]
    BudzetniKorisnik,
    [Description("Inostrano Preduzeće")]
    InostranoPreduzece,
    [Description("Stambena Zajednica")]
    StambenaZajednica,
}
