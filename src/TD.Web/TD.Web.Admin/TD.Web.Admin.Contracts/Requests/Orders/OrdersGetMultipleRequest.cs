using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Requests;

namespace TD.Web.Admin.Contracts.Requests.Orders;

public class OrdersGetMultipleRequest : LSCoreSortableAndPageableRequest<OrdersSortColumnCodes.Orders>
{
    public long? UserId { get; set; }
    public OrderStatus[]? Status { get; set; }
}