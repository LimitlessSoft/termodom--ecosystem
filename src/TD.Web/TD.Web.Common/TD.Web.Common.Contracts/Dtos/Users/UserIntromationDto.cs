using TD.Web.Public.Contrats.Dtos.Users;

namespace TD.Web.Common.Contracts.Dtos.Users
{
    public class UserInformationDto
    {
        public bool IsLogged
        {
            get => UserData != null;
        }
        public UserDataDto? UserData { get; set; }
    }
}
