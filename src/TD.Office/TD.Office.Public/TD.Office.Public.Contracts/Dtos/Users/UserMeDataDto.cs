namespace TD.Office.Public.Contracts.Dtos.Users;

public class UserMeDataDto
{
	public long Id { get; set; }
	public string? Username { get; set; }
	public string? Nickname { get; set; }
	public int? StoreId { get; set; }
	public int? VpStoreId { get; set; }
	public bool IsAdmin { get; set; }
}
