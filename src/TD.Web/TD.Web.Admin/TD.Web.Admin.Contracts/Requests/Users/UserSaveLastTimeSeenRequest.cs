using TD.Core.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Users
{
    public class UserSaveLastTimeSeenRequest : SaveRequest
    {
        public DateTime? LastTimeSeen { get; set; } = DateTime.UtcNow;

        public UserSaveLastTimeSeenRequest() : base() { }
        public UserSaveLastTimeSeenRequest(int userId) : base(userId) { }
    }
}
