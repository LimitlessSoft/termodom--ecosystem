namespace TD.Office.Public.Contracts.Dtos.Users
{
    public class UserMeDto
    {
        public bool IsLogged
        {
            get => UserData != null;
        }
        public UserMeDataDto? UserData { get; set; }
    }
}
