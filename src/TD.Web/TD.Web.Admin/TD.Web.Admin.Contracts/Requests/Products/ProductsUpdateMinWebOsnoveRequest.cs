namespace TD.Web.Admin.Contracts.Requests.Products;

public class ProductsUpdateMinWebOsnoveRequest
{
	public class MinItem
	{
		public long ProductId { get; set; }
		public decimal MinWebOsnova { get; set; }
	}

	public List<MinItem> Items { get; set; }
}
