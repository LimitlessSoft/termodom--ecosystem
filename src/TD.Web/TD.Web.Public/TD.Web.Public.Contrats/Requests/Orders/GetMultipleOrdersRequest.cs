using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Enums;
using LSCore.Contracts.Requests;

namespace TD.Web.Public.Contracts.Requests.Orders;

public class GetMultipleOrdersRequest : LSCoreSortableAndPageableRequest<OrdersSortColumnCodes.Orders>
{
    public OrderStatus[]? Status { get; set; }
}