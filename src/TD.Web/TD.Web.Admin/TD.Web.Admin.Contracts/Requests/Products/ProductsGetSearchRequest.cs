namespace TD.Web.Admin.Contracts.Requests.Products
{
	public class ProductsGetSearchRequest : ProductsGetMultipleRequest
	{
		public string? SearchTerm { get; set; }
	}
}
