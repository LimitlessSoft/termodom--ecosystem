namespace TD.Office.Public.Contracts.Requests.Users;

public class UsersUpdateNicknameRequest
{
	public long? Id { get; set; }
	public string Nickname { get; set; }
}
