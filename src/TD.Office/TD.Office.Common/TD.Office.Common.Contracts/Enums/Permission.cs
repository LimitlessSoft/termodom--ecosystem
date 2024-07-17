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
}