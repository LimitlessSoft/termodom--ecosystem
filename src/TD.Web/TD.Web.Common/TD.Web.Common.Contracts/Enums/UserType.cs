using System.ComponentModel;

namespace TD.Web.Common.Contracts.Enums;

public enum UserType
{
	[Description("Korisnik")]
	User = 0,

	[Description("Admin")]
	Admin = 1,

	[Description("Super Admin")]
	SuperAdmin = 2,

	[Description("Gost")]
	Guest = 3
}
