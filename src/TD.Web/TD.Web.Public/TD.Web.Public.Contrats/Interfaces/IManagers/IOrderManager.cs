using LSCore.Contracts.IManagers;
using LSCore.Contracts.Responses;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Requests.Orders;

namespace TD.Web.Public.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : ILSCoreBaseManager
    {
        LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(GetMultipleOrdersRequest request);
    }
}
