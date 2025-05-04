using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Public.Contracts.Requests.Users;

public class UsersUpdatePermissionRequest
{
	public long? Id { get; set; }
	public Permission? Permission { get; set; }
	public bool IsGranted { get; set; }
}
