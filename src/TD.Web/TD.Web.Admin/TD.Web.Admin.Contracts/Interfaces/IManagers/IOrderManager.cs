using LSCore.Contracts.IManagers;
using LSCore.Contracts.Responses;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Requests.Orders;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : ILSCoreBaseManager
    {
        LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request);
    }
}
