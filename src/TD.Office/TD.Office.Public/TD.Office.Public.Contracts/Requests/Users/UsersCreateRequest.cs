using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Requests.Users
{
    public class UsersCreateRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Nickname { get; set; }
    }
}