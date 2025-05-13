using TD.Web.Common.Contracts.Interfaces;

namespace TD.Web.Common.Contracts.Requests.Users
{
	public class UserRegisterRequest : IPassword
	{
		public string Username { get; set; }
		public string Password { get; set; }
		public string Nickname { get; set; }
		public DateTime DateOfBirth { get; set; }
		public string Mobile { get; set; }
		public string Address { get; set; }
		public long CityId { get; set; }
		public long FavoriteStoreId { get; set; }
		public string? Mail { get; set; }
	}
}
