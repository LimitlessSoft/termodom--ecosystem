using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Users;

public class UsersUpdatePasswordRequest : LSCoreSaveRequest
{
    public string Password { get; set; }
}