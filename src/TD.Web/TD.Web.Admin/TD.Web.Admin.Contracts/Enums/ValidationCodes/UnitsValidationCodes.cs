using System.ComponentModel;

namespace TD.Web.Admin.Contracts.Enums.ValidationCodes
{
	public enum UnitsValidationCodes
	{
		[Description("Ovo ime je već zauzeto!")]
		UVC_001,

		[Description("Polje '{0}' može sadržati samo slova i brojeve!")]
		UVC_002,

		[Description("Ne možete obrisati jedinicu mere koja je vezana za proizvod!")]
		UVC_003
	}
}
