using LSCore.SortAndPage.Contracts;
using TD.Komercijalno.Contracts.Entities;

namespace TD.Komercijalno.Contracts.Enums.SortColumnCodes;

public static class PartneriSortColumCodes
{
	public enum Partneri
	{
		PPID
	}

	public static Dictionary<Partneri, LSCoreSortRule<Partner>> PartneriSortRules =
		new() { { Partneri.PPID, new LSCoreSortRule<Partner>(x => x.Ppid) } };
}
