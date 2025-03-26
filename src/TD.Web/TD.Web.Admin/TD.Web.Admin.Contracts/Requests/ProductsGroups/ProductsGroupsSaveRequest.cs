namespace TD.Web.Admin.Contracts.Requests.ProductsGroups;

public class ProductsGroupsSaveRequest
{
	public long? Id { get; set; }
	public string Name { get; set; }
	public int? ParentGroupId { get; set; }
	public string? WelcomeMessage { get; set; }
}
