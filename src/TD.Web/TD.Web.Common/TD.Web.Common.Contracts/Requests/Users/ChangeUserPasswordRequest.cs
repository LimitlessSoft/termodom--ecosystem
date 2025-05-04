using TD.Web.Common.Contracts.Interfaces;

namespace TD.Web.Common.Contracts.Requests.Users
{
	public class ChangeUserPasswordRequest : IPassword
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
