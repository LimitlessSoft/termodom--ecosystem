using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Public.Contracts.Requests.Cart;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Enums;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using Microsoft.AspNetCore.Http;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using TD.Web.Common.Contracts;
using LSCore.Domain.Managers;

namespace TD.Web.Public.Domain.Managers;

public class CartManager : LSCoreManagerBase<CartManager>, ICartManager
{
    private readonly IOrderManager _orderManager;
    private readonly IOfficeServerApiManager _officeServerApiManager;

    public CartManager(ILogger<CartManager> logger, WebDbContext dbContext, IOrderManager orderManager, IHttpContextAccessor httpContextAccessor, IOfficeServerApiManager officeServerApiManager)
        : base(logger, dbContext)
    {
        _orderManager = orderManager;
            
        _officeServerApiManager = officeServerApiManager;
    }

    private void RecalculateAndApplyOrderItemsPrices(RecalculateAndApplyOrderItemsPricesCommandRequest request)
    {
        if (request == null)
            throw new LSCoreBadRequestException();

        var orderItems = Queryable<OrderItemEntity>()
            .Where(x =>
                x.IsActive &&
                x.OrderId == request!.Id)
            .Include(x => x.Product)
            .ThenInclude(x => x.Price);

        if(request.UserId == null)
            CalculateAndApplyOneTimePrices();
        else
            CalculateAndApplyUserPrices();

        Update(orderItems);
        return;

        #region Inner methods
        void CalculateAndApplyOneTimePrices()
        {
            var totalCartValueWithoutDiscount = orderItems.Sum(x => x.Product.Price.Max * x.Quantity);

            foreach (var item in orderItems)
            {
                item.PriceWithoutDiscount = item.Product.Price.Max;
                item.Price = PricesHelpers.CalculateOneTimeCartPrice(item.Product.Price.Min, item.Product.Price.Max, totalCartValueWithoutDiscount);
            }
        }

        void CalculateAndApplyUserPrices()
        {
            var user = Queryable<UserEntity>()
                .Include(x => x.ProductPriceGroupLevels)
                .FirstOrDefault(x => x.Id == request.UserId);

            foreach (var item in orderItems)
            {
                item.PriceWithoutDiscount = item.Product.Price.Max;
                item.Price = PricesHelpers.CalculateProductPriceByLevel(
                    item.Product.Price.Min,
                    item.Product.Price.Max,
                    user?.ProductPriceGroupLevels.FirstOrDefault(z => z.ProductPriceGroupId == item.Product.ProductPriceGroupId)?.Level ?? 0);
            }
        }
        #endregion
    }
    
    public void Checkout(CheckoutRequest request)
    {
        request.Validate();
        
        var currentOrder = _orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);

        RecalculateAndApplyOrderItemsPrices(new RecalculateAndApplyOrderItemsPricesCommandRequest()
        {
            Id = currentOrder.Id,
            UserId = CurrentUser?.Id
        });

        if (currentOrder.Items.IsEmpty())
            throw new LSCoreBadRequestException();
            
        #region Check if user is not guest

        if (CurrentUser != null)
        {
            var currentUser = Queryable<UserEntity>()
                .FirstOrDefault(x => x.Id == CurrentUser.Id);

            if (currentUser == null)
                throw new LSCoreNotFoundException();

            if (currentUser.Type == UserType.Guest)
                throw new LSCoreBadRequestException(UsersValidationCodes.UVC_029.GetDescription()!);
        }
        #endregion

        #region Entity Mapping
        if (CurrentUser == null)
            currentOrder.OrderOneTimeInformation = new OrderOneTimeInformationEntity()
            {
                Name = request.Name,
                Mobile = request.Mobile
            };
        currentOrder.Status = OrderStatus.PendingReview;
        currentOrder.StoreId = request.StoreId;
        currentOrder.Note = request.Note;
        currentOrder.PaymentTypeId = request.PaymentTypeId;
        currentOrder.CheckedOutAt = DateTime.UtcNow;
        #endregion

        Update(currentOrder);

        #region QueueSMS
        var storeName = "Unknown";

        if (request.StoreId == -5)
        {
            storeName = "Dostava";
        }
        else
        {
            var storeQuery = Queryable<StoreEntity>()
                .Where(x => x.Id == request.StoreId);
            storeName = storeQuery.FirstOrDefault()?.Name ?? storeName;                    
        }
            
        _officeServerApiManager.SmsQueueAsync(new SMSQueueRequest()
        {
            Text = $"Nova pourzbina je zakljucena. Mesto preuzimanja: {storeName}",
        });
        #endregion
    }

    public CartGetDto Get(CartGetRequest request)
    {
        var order = _orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);

        var orderWithItems = Queryable<OrderEntity>()
            .Where(x => x.IsActive && x.Id == order.Id) 
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Unit)
            .FirstOrDefault();

        if (orderWithItems == null)
            throw new LSCoreNotFoundException();

        // Recalculate and apply outdated prices if needed
        RecalculateAndApplyOrderItemsPrices(new RecalculateAndApplyOrderItemsPricesCommandRequest()
        {
            Id = orderWithItems.Id,
            UserId = CurrentUser?.Id
        });

        var dto = new CartGetDto();
        dto = orderWithItems.ToDto<OrderEntity, CartGetDto>();
        dto.FavoriteStoreId = orderWithItems.User.Id == 0 ? Constants.DefaultFavoriteStoreId : orderWithItems.User.FavoriteStoreId;
        return dto;
    }

    public CartGetCurrentLevelInformationDto GetCurrentLevelInformation(CartCurrentLevelInformationRequest request)
    {
        var orderResponse = _orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);

        var orderWithItems = Queryable<OrderEntity>()
            .Where(x => x.IsActive &&
                        x.Id == orderResponse.Id)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .ThenInclude(x => x.Price)
            .FirstOrDefault();

        if(orderWithItems == null)
            throw new LSCoreNotFoundException();

        if (CurrentUser != null || orderWithItems.Items.IsEmpty())
            throw new LSCoreBadRequestException();

        var totalCartValueWithoutDiscount = orderWithItems.Items.Sum(x => x.Price * x.Quantity);
        return new CartGetCurrentLevelInformationDto()
        {
            CurrentLevel = PricesHelpers.CalculateCartLevel(totalCartValueWithoutDiscount),
            NextLevelValue = totalCartValueWithoutDiscount + PricesHelpers.CalculateValueToNextLevel(totalCartValueWithoutDiscount)
        };
    }
}