namespace TD.Web.Admin.Contracts.Requests.Orders
{
	public class OrdersPutStoreIdRequest
	{
		public string OneTimeHash { get; set; }
		public short StoreId { get; set; }
	}
}
