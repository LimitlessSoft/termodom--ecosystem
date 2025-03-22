using LSCore.SortAndPage.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Enums.SortColumnCodes;

public static class ProracuniSortColumnCodes
{
	public enum Proracuni
	{
		Id,
		CreatedAt
	}

	public static Dictionary<Proracuni, LSCoreSortRule<ProracunEntity>> ProracuniSortRules =
		new()
		{
			{ Proracuni.Id, new LSCoreSortRule<ProracunEntity>(x => x.Id) },
			{ Proracuni.CreatedAt, new LSCoreSortRule<ProracunEntity>(x => x.CreatedAt) },
		};
}
