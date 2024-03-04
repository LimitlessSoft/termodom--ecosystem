using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Requests.Users
{
    public class PutUserTypeRequest
    {
        public string Username { get; set; }
        public UserType Type { get; set; }
    }
}