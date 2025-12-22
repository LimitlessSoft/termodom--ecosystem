using System.ComponentModel;

namespace TD.Web.Common.Contracts.Enums
{
	public enum BlogStatus
	{
		[Description("Nacrt")]
		Draft = 0,

		[Description("Objavljeno")]
		Published = 1
	}
}
