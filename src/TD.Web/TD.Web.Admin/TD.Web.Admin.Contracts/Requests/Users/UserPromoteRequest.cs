using TD.Core.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Users
{
    public class UserPromoteRequest : SaveRequest
    {
        public UserType Type { get; set; }
    }
}
