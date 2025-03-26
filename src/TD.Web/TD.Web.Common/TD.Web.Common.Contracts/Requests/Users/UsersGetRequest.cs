using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Common.Contracts.Requests.Users;

public class UsersGetRequest : LSCoreSortableAndPageableRequest<UsersSortColumnCodes.Users>
{
	public bool? HasReferent { get; set; }
	public bool? IsActive { get; set; }
}
