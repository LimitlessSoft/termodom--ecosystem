using TD.Core.Contracts.Requests;
using TD.Web.Contracts.Enums;

namespace TD.Web.Contracts.Requests.Users
{
    public class UserPromoteRequest : SaveRequest
    {
        public UserType Type { get; set; }
    }
}
