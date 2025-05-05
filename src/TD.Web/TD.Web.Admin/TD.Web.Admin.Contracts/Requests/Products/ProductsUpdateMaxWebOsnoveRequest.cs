namespace TD.Web.Admin.Contracts.Requests.Products
{
	public class ProductsUpdateMaxWebOsnoveRequest
	{
		public class MaxItem
		{
			public long ProductId { get; set; }
			public decimal MaxWebOsnova { get; set; }
		}

		public List<MaxItem> Items { get; set; }
	}
}
