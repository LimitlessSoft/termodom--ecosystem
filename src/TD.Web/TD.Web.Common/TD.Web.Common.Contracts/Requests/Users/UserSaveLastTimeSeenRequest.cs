using LSCore.Contracts.Requests;

namespace TD.Web.Common.Contracts.Requests.Users
{
    public class UserSaveLastTimeSeenRequest : LSCoreSaveRequest
    {
        public DateTime? LastTimeSeen { get; set; } = DateTime.UtcNow;

        public UserSaveLastTimeSeenRequest() : base() { }
        public UserSaveLastTimeSeenRequest(int userId) : base(userId) { }
    }
}
