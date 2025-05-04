namespace TD.Web.Admin.Contracts.Dtos.Products
{
	public class ProductsGetGroupItemDto
	{
		public long Id { get; set; }
		public string Name { get; set; }
		public ProductsGetGroupItemDto? ParentGroup { get; set; }
	}
}
