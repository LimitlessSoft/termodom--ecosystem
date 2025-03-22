using LSCore.SortAndPage.Contracts;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Public.Contracts.Enums.SortColumnCodes
{
	public static class UsersSortColumnCodes
	{
		public enum Users
		{
			Id,
			Username,
			Nickname,
		}

		public static Dictionary<Users, LSCoreSortRule<UserEntity>> UsersSortRules =
			new()
			{
				{ Users.Id, new LSCoreSortRule<UserEntity>(x => x.Id) },
				{ Users.Username, new LSCoreSortRule<UserEntity>(x => x.Username) },
				{ Users.Nickname, new LSCoreSortRule<UserEntity>(x => x.Nickname) }
			};
	}
}
