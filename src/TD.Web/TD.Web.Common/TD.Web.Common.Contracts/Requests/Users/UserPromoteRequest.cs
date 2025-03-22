using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Requests.Users;

public class UserPromoteRequest
{
	public long? Id { get; set; }
	public UserType Type { get; set; }
}
