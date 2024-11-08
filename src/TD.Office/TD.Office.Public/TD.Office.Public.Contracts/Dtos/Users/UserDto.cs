namespace TD.Office.Public.Contracts.Dtos.Users;

public class UserDto
{
    public long Id { get; set; }
    public string Username { get; set; }
    public string Nickname { get; set; }
    public decimal MaxRabatMPDokumenti { get; set; }
    public decimal MaxRabatVPDokumenti { get; set; }
}