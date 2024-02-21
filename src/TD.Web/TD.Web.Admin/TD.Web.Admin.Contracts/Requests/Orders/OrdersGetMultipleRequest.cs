using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Admin.Contracts.Requests.Orders
{
    public class OrdersGetMultipleRequest : LSCoreSortablePageableRequest<OrdersSortColumnCodes.Orders>
    {
        public OrderStatus[]? Status { get; set; }
    }
}
