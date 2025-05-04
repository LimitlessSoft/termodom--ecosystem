using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Dtos.Users;

public class UsersGetDto
{
	public UserType UserTypeId { get; set; }
	public string UserType { get; set; }
	public long Id { get; set; }
	public string Nickname { get; set; }
	public string Username { get; set; }
	public string Mobile { get; set; }
	public bool IsActive { get; set; }
	public long FavoriteStoreId { get; set; }
	public long? ProfessionId { get; set; }
	public long CityId { get; set; }
	public int NumberOfOrdersLastThreeMonths { get; set; }
	public bool NeverOrdered { get; set; }
}
