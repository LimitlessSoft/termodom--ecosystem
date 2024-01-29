using LSCore.Contracts.Http;
using LSCore.Contracts.IManagers;
using LSCore.Contracts.Requests;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Common.Contracts.Requests.Orders;

namespace TD.Web.Common.Contracts.Interfaces.IManagers
{
    public interface IOrderManager : ILSCoreBaseManager
    {
        /// <summary>
        /// Get current order associated with user.
        /// If CurrentUser is null, then oneTimeHash should be passed.
        /// If both are null, new order will be created with new oneTimeHash.
        /// If no order is found under current user, new order will be created for that user.
        /// If no order is found under current oneTimeHash, new order will be created with new (not the passed one) oneTimeHash.
        /// </summary>
        /// <param name="oneTimeHash"></param>
        /// <returns></returns>
        LSCoreResponse<OrderEntity> GetOrCreateCurrentOrder(string? oneTimeHash = null);

        LSCoreResponse<string> AddItem(OrdersAddItemRequest request);
        LSCoreResponse<decimal> GetTotalValueWithoutDiscount(LSCoreIdRequest request);
        LSCoreResponse RemoveItem(RemoveOrderItemRequest request);
        LSCoreResponse ChangeItemQuantity(ChangeItemQuantityRequest request);
        LSCoreResponse<OrderEntity> GetCurrentActiveOrder(string? onetTimeHash = null);
    }
}
