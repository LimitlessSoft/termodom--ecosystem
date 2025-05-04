using System.ComponentModel;
using TD.Web.Admin.Contracts.Requests.Statistics;

namespace TD.Web.Admin.Contracts.Enums.ValidationCodes
{
	public enum ProductsStatisticsValidationCodes
	{
		[Description(
			$"'{nameof(ProductsStatisticsRequest.DateFromUtc)}' mora biti manji od datuma '{nameof(ProductsStatisticsRequest.DateToUtc)}'."
		)]
		PSVC_001,

		[Description($"Datum '{nameof(ProductsStatisticsRequest.DateFromUtc)}' mora biti izabran.")]
		PSVC_002,

		[Description($"Datum '{nameof(ProductsStatisticsRequest.DateToUtc)}' mora biti izabran.")]
		PSVC_003
	}
}
