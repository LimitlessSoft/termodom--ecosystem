using System.ComponentModel;
using TD.Web.Common.Contracts.Attributes;

namespace TD.Web.Common.Contracts.Enums;

public enum Permission
{
	[Description("Pristup aplikaciji")]
	Access,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[PermissionGroup(LegacyConstants.PermissionGroup.Products)]
	[Description("Admin - Proizvodi - Pristup modulu")]
	Admin_Products_Access,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[Description("Admin - Porudzbine - Pristup modulu")]
	Admin_Orders_Access,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[Description("Admin - Korisnici - Pristup modulu")]
	Admin_Users_Access,

	[PermissionGroup(LegacyConstants.PermissionGroup.NavBar)]
	[Description("Admin - Podesavanja - Pristup modulu")]
	Admin_Settings_Access,

	[PermissionGroup(LegacyConstants.PermissionGroup.Products)]
	[Description(
		"Admin - Proizvodi - Moze da menja sve proizvode (zanemaruje prava nad grupama proizvoda)"
	)]
	Admin_Products_EditAll,

	[PermissionGroup(LegacyConstants.PermissionGroup.Products)]
	[Description("Admin - Proizvodi - Moze da menja 'src'")]
	Admin_Products_EditSrc,

	[PermissionGroup(LegacyConstants.PermissionGroup.Products)]
	[Description("Admin - Proizvodi - Moze da menja 'Meta Tags'")]
	Admin_Products_EditMetaTags,
}
