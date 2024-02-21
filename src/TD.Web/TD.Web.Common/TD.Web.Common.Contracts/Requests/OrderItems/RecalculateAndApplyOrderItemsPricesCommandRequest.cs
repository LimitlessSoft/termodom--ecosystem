using LSCore.Contracts.Requests;

namespace TD.Web.Common.Contracts.Requests.OrderItems
{
    public class RecalculateAndApplyOrderItemsPricesCommandRequest : LSCoreIdRequest
    {
        public int? UserId { get; set; }
    }
}
