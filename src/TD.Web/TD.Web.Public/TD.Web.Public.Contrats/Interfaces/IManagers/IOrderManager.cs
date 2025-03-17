using LSCore.Common.Contracts;
using LSCore.SortAndPage.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Requests.Orders;

namespace TD.Web.Public.Contracts.Interfaces.IManagers;

public interface IOrderManager
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
	OrderEntity GetOrCreateCurrentOrder(string? oneTimeHash = null);
	string AddItem(OrdersAddItemRequest request);
	decimal GetTotalValueWithoutDiscount(LSCoreIdRequest request);
	void RemoveItem(RemoveOrderItemRequest request);
	void ChangeItemQuantity(ChangeItemQuantityRequest request);
	LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(GetMultipleOrdersRequest request);
	OrdersInfoDto GetOrdersInfo();
	OrderGetSingleDto GetSingle(GetSingleOrderRequest request);
}
