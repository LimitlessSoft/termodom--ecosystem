using LSCore.Common.Contracts;

namespace TD.Web.Common.Contracts.Requests.OrderItems;

public class RecalculateAndApplyOrderItemsPricesCommandRequest : LSCoreIdRequest
{
	public long? UserId { get; set; }
}
