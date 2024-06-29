namespace TD.Web.Common.Contracts.Dtos.Users;
public class UserInformationDto
{
    public bool IsLogged => UserData != null;
    public UserDataDto? UserData { get; set; }
}
