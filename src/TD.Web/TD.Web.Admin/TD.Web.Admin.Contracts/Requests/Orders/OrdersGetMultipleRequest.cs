using LSCore.Contracts.Requests;
using TD.Web.Admin.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Contracts.Requests.Orders
{
    public class OrdersGetMultipleRequest : LSCoreSortablePageableRequest<OrdersSortColumnCodes.Orders>
    {
        public OrderStatus[]? Status { get; set; }
    }
}
