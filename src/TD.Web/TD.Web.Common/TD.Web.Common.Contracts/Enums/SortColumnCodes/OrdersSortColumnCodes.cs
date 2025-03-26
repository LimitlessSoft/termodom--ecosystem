using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes;

public static class OrdersSortColumnCodes
{
	public enum Orders
	{
		Date
	}

	public static Dictionary<Orders, LSCoreSortRule<OrderEntity>> OrdersSortRules =
		new() { { Orders.Date, new LSCoreSortRule<OrderEntity>(x => x.CheckedOutAt!.Value) } };
}
