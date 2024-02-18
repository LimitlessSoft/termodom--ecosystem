using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Responses;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Requests.Orders;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : ILSCoreBaseManager
    {
        LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request);
        LSCoreResponse<OrdersGetDto> GetSingle(OrdersGetSingleRequest request);
        LSCoreResponse PutStoreId(OrdersPutStoreIdRequest request);
        LSCoreResponse PutStatus(OrdersPutStatusRequest request);
        LSCoreResponse PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request);
    }
}
