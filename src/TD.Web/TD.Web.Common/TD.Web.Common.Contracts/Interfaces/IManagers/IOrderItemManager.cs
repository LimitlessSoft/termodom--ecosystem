using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.Interfaces.IManagers;

public interface IOrderItemManager
{
    OrderItemEntity Insert(OrderItemEntity request);
    bool Exists(OrderItemExistsRequest request);
    void Delete(DeleteOrderItemRequest request);
    void ChangeQuantity(ChangeOrderItemQuantityRequest request);
    OrderItemEntity GetOrderItem(GetOrderItemRequest request);
}