using LSCore.Auth.Contracts;
using LSCore.Common.Extensions;
using LSCore.Exceptions;
using LSCore.Mapper.Domain;
using LSCore.Validation.Domain;
using Microsoft.EntityFrameworkCore;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Common.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Domain.Managers;

public class CartManager(
	IOrderManager orderManager,
	LSCoreAuthContextEntity<string> contextEntity,
	IOrderRepository orderRepository,
	IOrderItemRepository orderItemRepository,
	IOfficeServerApiManager officeServerApiManager,
	IUserRepository userRepository,
	ISettingRepository settingRepository,
	IStoreRepository storeRepository
) : ICartManager
{
	private void RecalculateAndApplyOrderItemsPrices(
		RecalculateAndApplyOrderItemsPricesCommandRequest request
	)
	{
		if (request == null)
			throw new LSCoreBadRequestException();

		var orderItems = orderItemRepository
			.GetMultiple()
			.Where(x => x.OrderId == request.Id)
			.Include(x => x.Product)
			.ThenInclude(x => x.Price)
			.ToList();

		if (orderItems.Count == 0)
			return;
		if (request.UserId == null)
			CalculateAndApplyOneTimePrices();
		else
			CalculateAndApplyUserPrices();

		foreach (var orderItemEntity in orderItems)
			orderItemRepository.UpdateOrInsert(orderItemEntity);

		return;

		#region Inner methods
		void CalculateAndApplyOneTimePrices()
		{
			var totalCartValueWithoutDiscount = orderItems.Sum(x =>
				x.Product.Price.Max * x.Quantity
			);

			foreach (var item in orderItems)
			{
				item.PriceWithoutDiscount = item.Product.Price.Max;
				item.Price = PricesHelpers.CalculateOneTimeCartPrice(
					item.Product.Price.Min,
					item.Product.Price.Max,
					totalCartValueWithoutDiscount
				);
			}
		}

		void CalculateAndApplyUserPrices()
		{
			var user = userRepository
				.GetMultiple()
				.Include(x => x.ProductPriceGroupLevels)
				.FirstOrDefault(x => x.Id == request.UserId);

			foreach (var item in orderItems)
			{
				item.PriceWithoutDiscount = item.Product.Price.Max;
				item.Price = PricesHelpers.CalculateProductPriceByLevel(
					item.Product.Price.Min,
					item.Product.Price.Max,
					user?.ProductPriceGroupLevels.FirstOrDefault(z =>
						z.ProductPriceGroupId == item.Product.ProductPriceGroupId
					)?.Level ?? 0
				);
			}
		}
		#endregion
	}

	public void Checkout(CheckoutRequest request)
	{
		request.Validate();

		var currentOrder = orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);
		var currentUser = contextEntity.IsAuthenticated ? userRepository.GetCurrentUser() : null;

		RecalculateAndApplyOrderItemsPrices(
			new RecalculateAndApplyOrderItemsPricesCommandRequest()
			{
				Id = currentOrder.Id,
				UserId = currentUser?.Id
			}
		);

		if (currentOrder.Items == null || currentOrder.Items.Count == 0)
			throw new LSCoreBadRequestException();

		#region Check if user is not guest

		if (currentUser is { Type: UserType.Guest })
			throw new LSCoreBadRequestException(UsersValidationCodes.UVC_029.GetDescription()!);
		#endregion

		#region Entity Mapping
		if (contextEntity.IsAuthenticated == false)
			currentOrder.OrderOneTimeInformation = new OrderOneTimeInformationEntity
			{
				Name = request.Name ?? throw new LSCoreBadRequestException(),
				Mobile = request.Mobile ?? throw new LSCoreBadRequestException()
			};
		else
		{
			currentOrder.CreatedBy = currentUser!.Id;
			currentOrder.OrderOneTimeInformation = null;
		}
		currentOrder.Status = OrderStatus.PendingReview;
		currentOrder.StoreId = request.StoreId;
		currentOrder.Note = request.Note;
		currentOrder.PaymentTypeId = request.PaymentTypeId;
		currentOrder.CheckedOutAt = DateTime.UtcNow;
		currentOrder.DeliveryAddress = request.DeliveryAddress;
		#endregion

		orderRepository.Update(currentOrder);

		#region QueueSMS

		if (settingRepository.GetValue<bool>(SettingKey.SMS_SEND_ZAKLJUCENA_PORUDZBINA))
		{
			var storeName = "Unknown";

			if (request.StoreId == -5)
			{
				storeName = "Dostava";
			}
			else
			{
				var storeQuery = storeRepository.GetMultiple().Where(x => x.Id == request.StoreId);
				storeName = storeQuery.FirstOrDefault()?.Name ?? storeName;
			}

			officeServerApiManager.SmsQueueAsync(
				new SMSQueueRequest()
				{
					Text = $"Nova pourzbina je zakljucena. Mesto preuzimanja: {storeName}",
				}
			);
		}
		#endregion
	}

	public CartGetDto Get(CartGetRequest request)
	{
		var order = orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);

		var orderWithItems = orderRepository
			.GetMultiple()
			.Where(x => x.Id == order.Id)
			.Include(x => x.User)
			.Include(x => x.Items)
			.ThenInclude(x => x.Product)
			.ThenInclude(x => x.Unit)
			.FirstOrDefault();

		if (orderWithItems == null)
			throw new LSCoreNotFoundException();

		var currentUser = contextEntity.IsAuthenticated ? userRepository.GetCurrentUser() : null;

		// Recalculate and apply outdated prices if needed
		RecalculateAndApplyOrderItemsPrices(
			new RecalculateAndApplyOrderItemsPricesCommandRequest()
			{
				Id = orderWithItems.Id,
				UserId = currentUser?.Id
			}
		);

		return orderWithItems.ToMapped<OrderEntity, CartGetDto>();
	}

	public CartGetCurrentLevelInformationDto GetCurrentLevelInformation(
		CartCurrentLevelInformationRequest request
	)
	{
		var orderResponse = orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);

		var orderWithItems = orderRepository
			.GetMultiple()
			.Where(x => x.Id == orderResponse.Id)
			.Include(x => x.Items)
			.ThenInclude(x => x.Product)
			.ThenInclude(x => x.Price)
			.FirstOrDefault();

		if (orderWithItems == null)
			throw new LSCoreNotFoundException();

		if (
			contextEntity.IsAuthenticated
			|| orderWithItems.Items == null
			|| orderWithItems.Items.Count == 0
		)
			throw new LSCoreBadRequestException();

		var totalCartValueWithoutDiscount = orderWithItems.Items.Sum(x => x.Price * x.Quantity);
		return new CartGetCurrentLevelInformationDto()
		{
			CurrentLevel = PricesHelpers.CalculateCartLevel(totalCartValueWithoutDiscount),
			NextLevelValue =
				totalCartValueWithoutDiscount
				+ PricesHelpers.CalculateValueToNextLevel(totalCartValueWithoutDiscount)
		};
	}

	public CheckoutGetDto GetCheckout(string oneTimeHash)
	{
		var order = orderRepository
			.GetMultiple()
			.Where(x => x.OneTimeHash == oneTimeHash && x.IsActive)
			.Include(x => x.User)
			.Select(x => new { x.User, x.PaymentTypeId })
			.FirstOrDefault();

		if (order == null)
			throw new LSCoreNotFoundException();

		return new CheckoutGetDto
		{
			PaymentTypeId = order.PaymentTypeId,
			FavoriteStoreId = order.User.Id == 0
				? LegacyConstants.DefaultFavoriteStoreId
				: order.User.FavoriteStoreId
		};
	}
}
