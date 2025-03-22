namespace TD.Web.Common.Contracts.Requests.Users;

public class UserSaveLastTimeSeenRequest
{
	public long? Id { get; set; }
	public DateTime? LastTimeSeen { get; set; } = DateTime.UtcNow;

	public UserSaveLastTimeSeenRequest() { }

	public UserSaveLastTimeSeenRequest(long userId)
	{
		Id = userId;
	}
}
