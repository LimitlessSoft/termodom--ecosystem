using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.ProductsGroups;

public class ProductsGroupUpdateTypeRequest
{
	public long? Id { get; set; }
	public ProductGroupType Type { get; set; }
}
