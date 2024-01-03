using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Requests.OrderItems;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IOrderItemManager : ILSCoreBaseManager
    {
        LSCoreResponse<OrderItemEntity> Insert(OrderItemEntity request);
        LSCoreResponse<bool> Exists(OrderItemExistsRequest request);
        LSCoreResponse Delete(OrderItemEntity request);
        LSCoreResponse<OrderItemEntity> GetOrderItem(GetOrderItemRequest request);
    }
}
