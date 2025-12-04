using System.Linq.Expressions;
using LSCore.SortAndPage.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Enums.SortColumnCodes;

public static class PopisiSortColumnCodes
{
	public enum Popisi
	{
		Id,
	}

	public static Dictionary<Popisi, LSCoreSortRule<PopisDokumentEntity>> PopisiSortRules = new()
	{
		{ Popisi.Id, new LSCoreSortRule<PopisDokumentEntity>(x => x.Id) },
	};
}
