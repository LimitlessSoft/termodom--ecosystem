using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.SortAndPage.Contracts;
using LSCore.SortAndPage.Domain;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Helpers.Orders;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Orders;

namespace TD.Web.Public.Domain.Managers;

public class OrderManager(
	IOrderItemManager orderItemManager,
	IUserRepository userRepository,
	IOrderRepository orderRepository,
	IProductRepository productRepository,
	IPaymentTypeRepository paymentTypeRepository,
	LSCoreAuthContextEntity<string> contextEntity
) : IOrderManager
{
	public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(
		GetMultipleOrdersRequest request
	) =>
		orderRepository
			.GetMultiple()
			.Include(x => x.User)
			.Include(x => x.Items)
			.ThenInclude(x => x.Product)
			.Where(x =>
				x.User.Username == contextEntity.Identifier
				&& (
					request.Status == null
					|| request.Status.Length == 0
					|| request.Status!.Contains(x.Status)
				)
			)
			.ToSortedAndPagedResponse<OrderEntity, OrdersSortColumnCodes.Orders, OrdersGetDto>(
				request,
				OrdersSortColumnCodes.OrdersSortRules,
				x => x.ToMapped<OrderEntity, OrdersGetDto>()
			);

	public string AddItem(OrdersAddItemRequest request)
	{
		var product = productRepository
			.GetMultiple()
			.Where(x => x.Id == request.ProductId)
			.Include(x => x.Price)
			.FirstOrDefault();

		if (product == null)
			throw new LSCoreNotFoundException();

		var order = GetOrCreateCurrentOrder(request.OneTimeHash);

		var orderItemExists = orderItemManager.Exists(
			new OrderItemExistsRequest() { OrderId = order.Id, ProductId = product.Id }
		);

		if (orderItemExists)
			throw new LSCoreBadRequestException(OrdersValidationCodes.OVC_001.GetDescription()!);

		var newOrderItem = orderItemManager.Insert(
			new OrderItemEntity()
			{
				VAT = product.VAT,
				OrderId = order.Id,
				ProductId = request.ProductId,
				Quantity = request.Quantity,
				Price = 0, // Price will be calculated later when user enter the cart
				PriceWithoutDiscount = 0, // Price will be calculated later when user enter the cart
			}
		);

		return order.OneTimeHash;
	}

	public void ChangeItemQuantity(ChangeItemQuantityRequest request)
	{
		var currentOrder = GetOrCreateCurrentOrder(request.OneTimeHash);

		orderItemManager.ChangeQuantity(
			new ChangeOrderItemQuantityRequest()
			{
				OrderId = currentOrder.Id,
				ProductId = request.ProductId,
				Quantity = request.Quantity
			}
		);
	}

	/// <inheritdoc/>
	public OrderEntity GetOrCreateCurrentOrder(string? oneTimeHash = null)
	{
		var order = orderRepository
			.GetMultiple()
			.FirstOrDefault(x =>
				x.Status == OrderStatus.Open
				&& (!string.IsNullOrWhiteSpace(oneTimeHash) && x.OneTimeHash == oneTimeHash)
			);

		if (order == null)
		{
			// Todo: make so client can set default payment type for order by himself through the admin UI
			var paymentTypeResponse =
				contextEntity.IsAuthenticated == false
					? paymentTypeRepository.GetMultiple().FirstOrDefault(x => x.IsActive)
					: userRepository
						.GetMultiple()
						.Include(x => x.DefaultPaymentType)
						.FirstOrDefault(x => x.Username == contextEntity.Identifier)
						?.DefaultPaymentType;
			if (paymentTypeResponse == null)
				throw new LSCoreNotFoundException();

			var orderEntity = new OrderEntity
			{
				Status = OrderStatus.Open,
				OneTimeHash = OrdersHelpers.GenerateOneTimeHash(),
				StoreId = -5,
				PaymentTypeId = paymentTypeResponse.Id
			};

			if (contextEntity.IsAuthenticated)
			{
				var currentUser = userRepository.GetCurrentUser();
				orderEntity.CreatedBy = currentUser.Id;
			}

			orderRepository.Insert(orderEntity);
			return orderEntity;
		}

		return order;
	}

	public decimal GetTotalValueWithoutDiscount(LSCoreIdRequest request)
	{
		var order = orderRepository
			.GetMultiple()
			.Include(x => x.Items)
			.FirstOrDefault(x => x.Id == request.Id && x.IsActive);

		if (order == null)
			throw new LSCoreNotFoundException();

		var totalValue = 0m;

		order.Items.ForEach(x =>
		{
			totalValue += x.PriceWithoutDiscount * x.Quantity;
		});

		return totalValue;
	}

	public void RemoveItem(RemoveOrderItemRequest request)
	{
		var currentOrder = GetOrCreateCurrentOrder(request.OneTimeHash);

		orderItemManager.Delete(
			new DeleteOrderItemRequest()
			{
				OrderId = currentOrder.Id,
				ProductId = request.ProductId
			}
		);
	}

	public OrdersInfoDto GetOrdersInfo()
	{
		var user = userRepository.GetCurrentUser();

		var orders = orderRepository
			.GetMultiple()
			.Include(x => x.User)
			.Include(x => x.Items)
			.ThenInclude(x => x.Product)
			.Where(x => x.CreatedBy == user.Id && x.IsActive && x.Status == OrderStatus.Collected)
			.ToList();

		return new OrdersInfoDto
		{
			User = user.Username,
			NumberOfOrders = orders.Count,
			TotalDiscountValue = orders.Sum(order =>
				order.Items.Sum(item =>
					(item.PriceWithoutDiscount - item.Price) * item.Quantity * (item.VAT / 100 + 1)
				)
			)
		};
	}

	public OrderGetSingleDto GetSingle(GetSingleOrderRequest request)
	{
		var order = orderRepository
			.GetMultiple()
			.Where(x => x.OneTimeHash == request.OneTimeHash)
			.Include(x => x.Items)
			.ThenInclude(x => x.Product)
			.Include(x => x.OrderOneTimeInformation)
			.Include(x => x.Referent)
			.Include(x => x.User)
			.FirstOrDefault();

		if (order == null)
			throw new LSCoreNotFoundException();

		if (
			(
				order.OrderOneTimeInformation == null
				&& order.User.Identifier != contextEntity.Identifier
			)
			|| order.Status == OrderStatus.Open
		)
			throw new LSCoreBadRequestException();

		return order.ToMapped<OrderEntity, OrderGetSingleDto>();
	}
}
