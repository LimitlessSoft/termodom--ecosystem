namespace TD.Office.Public.Contracts.Dtos.Users;

public class UserMeDto
{
    public bool IsLogged => UserData != null;
    public UserMeDataDto? UserData { get; set; }
}
