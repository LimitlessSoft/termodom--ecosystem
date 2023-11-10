using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Users
{
    public class UserPromoteRequest : LSCoreSaveRequest
    {
        public UserType Type { get; set; }
    }
}
