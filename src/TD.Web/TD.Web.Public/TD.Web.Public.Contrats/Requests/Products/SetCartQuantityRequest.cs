using LSCore.Common.Contracts;

namespace TD.Web.Public.Contracts.Requests.Products;

public class SetCartQuantityRequest : LSCoreIdRequest
{
	public string? OneTimeHash { get; set; }
	public decimal Quantity { get; set; }
}
