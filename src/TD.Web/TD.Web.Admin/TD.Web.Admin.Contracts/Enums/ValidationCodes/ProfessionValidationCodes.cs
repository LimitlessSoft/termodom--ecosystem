using System.ComponentModel;

namespace TD.Web.Admin.Contracts.Enums.ValidationCodes
{
	public enum ProfessionValidationCodes
	{
		[Description("Ime ne moze biti prazno")]
		PVC_001,

		[Description("Ime mora biti kraće od {0} karaktera")]
		PVC_002,

		[Description("Ime mora biti duže od {0} karaktera")]
		PVC_003,

		[Description("Ime već postoji u bazi")]
		PVC_004,
	}
}
