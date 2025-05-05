using System.ComponentModel;

namespace TD.Web.Admin.Contracts.Enums.ValidationCodes
{
	public enum KomercijalnoWebProductLinksValidationCodes
	{
		[Description("Povezanost sa robaId ili webId već postoji na drugom proizvodu!")]
		KWPLVC_001,

		[Description("WebProduct ne postoji!")]
		KWPLVC_002
	}
}
