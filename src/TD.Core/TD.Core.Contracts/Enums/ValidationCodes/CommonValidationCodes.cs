using System.ComponentModel;

namespace TD.Core.Contracts.Enums.ValidationCodes
{
	public enum CommonValidationCodes
	{
		[Description("Vrednost '{0}' mora biti veca od '{1}'.")]
		COMM_001,

		[Description("Parametar '{0}' ne sme biti null!")]
		COMM_002,

		[Description("Parametar '{0}' mora biti kraci od {1} karaktera.")]
		COMM_003,

		[Description("Parametar '{0}' mora biti duzi od {1} karaktera.")]
		COMM_004,

		[Description("DBContext nije setovan!")]
		COMM_005,
	}
}
