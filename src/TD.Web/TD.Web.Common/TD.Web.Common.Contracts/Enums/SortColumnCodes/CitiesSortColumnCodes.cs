using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes
{
	public static class CitiesSortColumnCodes
	{
		public enum Cities
		{
			Name,
			Id
		}

		public static Dictionary<Cities, LSCoreSortRule<CityEntity>> CitiesSortRules =
			new()
			{
				{ Cities.Name, new LSCoreSortRule<CityEntity>(x => x.Name) },
				{ Cities.Id, new LSCoreSortRule<CityEntity>(x => x.Id) }
			};
	}
}
