using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Requests;

public class GetUsersProductPricesRequest
{
	public long UserId { get; set; }
	public long ProductId { get; set; }
	public ProductEntity? Product { get; set; }
}
