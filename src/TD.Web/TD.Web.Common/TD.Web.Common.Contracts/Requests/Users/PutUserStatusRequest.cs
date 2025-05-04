namespace TD.Web.Common.Contracts.Requests.Users
{
	public class PutUserStatusRequest
	{
		public string Username { get; set; }
		public bool IsActive { get; set; }
	}
}
