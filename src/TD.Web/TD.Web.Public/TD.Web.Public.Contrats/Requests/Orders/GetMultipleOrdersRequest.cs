using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;

namespace TD.Web.Public.Contracts.Requests.Orders
{
    public class GetMultipleOrdersRequest : LSCoreSortablePageableRequest<OrdersSortColumnCodes.Orders>
    {
    }
}
