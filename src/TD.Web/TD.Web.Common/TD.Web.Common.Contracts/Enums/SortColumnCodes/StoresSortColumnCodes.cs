using System.Linq.Expressions;
using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes
{
	public static class StoresSortColumnCodes
	{
		public enum Stores
		{
			Name
		}

		public static Dictionary<Stores, LSCoreSortRule<StoreEntity>> StoresSortRules =
			new() { { Stores.Name, new LSCoreSortRule<StoreEntity>(x => x.Name) } };
	}
}
