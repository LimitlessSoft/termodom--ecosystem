using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Users
{
    public class UsersUpdateNicknameRequest : LSCoreSaveRequest
    {
        public string Nickname { get; set; }
    }
}