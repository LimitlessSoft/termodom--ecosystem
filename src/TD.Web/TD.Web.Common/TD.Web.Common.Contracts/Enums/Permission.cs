using System.ComponentModel;
using TD.Web.Common.Contracts.Attributes;

namespace TD.Web.Common.Contracts.Enums;

public enum Permission
{
    [Description("Pristup aplikaciji")]
    Access,
    
    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [PermissionGroup(Constants.PermissionGroup.Products)]
    [Description("Admin - Proizvodi - Pristup modulu")]
    Admin_Products_Access,
    
    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [Description("Admin - Porudzbine - Pristup modulu")]
    Admin_Orders_Access,
    
    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [Description("Admin - Korisnici - Pristup modulu")]
    Admin_Users_Access,
    
    [PermissionGroup(Constants.PermissionGroup.NavBar)]
    [Description("Admin - Podesavanja - Pristup modulu")]
    Admin_Settings_Access,
    
    [PermissionGroup(Constants.PermissionGroup.Products)]
    [Description("Admin - Proizvodi - Moze da menja sve proizvode (zanemaruje prava nad grupama proizvoda)")]
    Admin_Products_EditAll,
    
    [PermissionGroup(Constants.PermissionGroup.Products)]
    [Description("Admin - Proizvodi - Moze da menja 'src'")]
    Admin_Products_EditSrc,
    
    [PermissionGroup(Constants.PermissionGroup.Products)]
    [Description("Admin - Proizvodi - Moze da menja 'Meta Tags'")]
    Admin_Products_EditMetaTags,
}