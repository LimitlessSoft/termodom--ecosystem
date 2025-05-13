using System.ComponentModel;

namespace TD.Web.Common.Contracts.Enums;

public enum ProductStockType
{
	[Description("Standard")]
	Standard,

	[Description("Samo velika stovarišta")]
	BigWarehouses,

	[Description("Tranzit")]
	Transit
}
