using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Common.Contracts.Requests.Users
{
    public class UsersGetRequest : LSCoreSortablePageableRequest<UsersSortColumnCodes.Users>
    {
        public bool? HasReferent { get; set; }
        public bool? IsActive { get; set; }
    }
}
