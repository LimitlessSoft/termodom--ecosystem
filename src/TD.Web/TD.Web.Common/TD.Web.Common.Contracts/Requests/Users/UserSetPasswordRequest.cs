namespace TD.Web.Common.Contracts.Requests.Users
{
	public class UserSetPasswordRequest
	{
		public string Password { get; set; }
		public string OldPassword { get; set; }
	}
}
