using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Enums.SortColumnCodes
{
	public static class UsersSortColumnCodes
	{
		public enum Users
		{
			Id,
			Username
		}

		public static Dictionary<Users, LSCoreSortRule<UserEntity>> UsersSortRules =
			new()
			{
				{ Users.Id, new LSCoreSortRule<UserEntity>(x => x.Id) },
				{ Users.Username, new LSCoreSortRule<UserEntity>(x => x.Username) }
			};
	}
}
