using LSCore.SortAndPage.Contracts;
using TD.Office.Public.Contracts.Enums.SortColumnCodes;

namespace TD.Office.Public.Contracts.Requests.Users;

public class UsersGetMultipleRequest
	: LSCoreSortableAndPageableRequest<UsersSortColumnCodes.Users> { }
