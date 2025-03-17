namespace TD.Office.Public.Contracts.Requests.Users;

public class UsersUpdatePasswordRequest
{
	public long? Id { get; set; }
	public string Password { get; set; }
}
