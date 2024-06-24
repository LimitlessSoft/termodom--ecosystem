using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Contracts.Enums.ValidationCodes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Public.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Helpers.Orders;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Entities;
using Microsoft.IdentityModel.Tokens;
using TD.Web.Common.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;

namespace TD.Web.Public.Domain.Managers;

public class OrderManager (ILogger<OrderManager> logger, WebDbContext dbContext, IOrderItemManager orderItemManager)
    : LSCoreManagerBase<OrderManager, OrderEntity>(logger, dbContext), IOrderManager
{
    public LSCoreSortedAndPagedResponse<OrdersGetDto> GetMultiple(GetMultipleOrdersRequest request)
    {
        if (CurrentUser == null)
            throw new LSCoreForbiddenException();

        return Queryable()
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Where(x => x.IsActive &&
                        x.User.Id == CurrentUser.Id &&
                        (request.Status.IsNullOrEmpty() || request.Status!.Contains(x.Status)))
            .ToSortedAndPagedResponse<OrderEntity, OrdersSortColumnCodes.Orders, OrdersGetDto>(request, OrdersSortColumnCodes.OrdersSortRules);
    }

    public string AddItem(OrdersAddItemRequest request)
    {
        var product = Queryable<ProductEntity>()
            .Where(x => x.Id == request.ProductId && x.IsActive)
            .Include(x => x.Price)
            .FirstOrDefault();
            
        if(product == null)
            throw new LSCoreNotFoundException();
            
        var order = GetOrCreateCurrentOrder(request.OneTimeHash);

        var orderItemExists = orderItemManager.Exists(new OrderItemExistsRequest()
        {
            OrderId = order.Id,
            ProductId = product.Id
        });

        if (orderItemExists)
            throw new LSCoreBadRequestException(OrdersValidationCodes.OVC_001.GetDescription()!);

        var newOrderItem = orderItemManager.Insert(new OrderItemEntity()
        {
            VAT = product.VAT,
            OrderId = order.Id,
            ProductId = request.ProductId,
            Quantity = request.Quantity,
            Price = 0, // Price will be calculated later when user enter the cart
            PriceWithoutDiscount = 0, // Price will be calculated later when user enter the cart
        });

        return order.OneTimeHash;
    }

    public void ChangeItemQuantity(ChangeItemQuantityRequest request)
    {
        var currentOrder = GetOrCreateCurrentOrder(request.OneTimeHash);

        orderItemManager.ChangeQuantity(new ChangeOrderItemQuantityRequest()
        {
            OrderId = currentOrder.Id,
            ProductId = request.ProductId,
            Quantity = request.Quantity
        });
    }

    /// <inheritdoc/>
    public OrderEntity GetOrCreateCurrentOrder(string? oneTimeHash = null)
    {
        var order = Queryable()
            .FirstOrDefault(x =>
                x.IsActive &&
                x.Status == OrderStatus.Open &&
                (CurrentUser == null ?
                    (string.IsNullOrWhiteSpace(oneTimeHash) ? false : x.OneTimeHash == oneTimeHash) :
                    x.CreatedBy == CurrentUser.Id));

        if (order == null)
        {
            // Todo: make so client can set default payment type for order by himself through the admin UI 
            var paymentTypeResponse = Queryable<PaymentTypeEntity>().FirstOrDefault(x => x.IsActive);
            if (paymentTypeResponse == null)
                throw new LSCoreNotFoundException();
                
            var orderEntity = new OrderEntity
            {
                Status = OrderStatus.Open,
                OneTimeHash = OrdersHelpers.GenerateOneTimeHash(),
                StoreId = -5,
                PaymentTypeId = paymentTypeResponse.Id
            };

            if (CurrentUser is { Id: not null })
                orderEntity.CreatedBy = CurrentUser.Id!.Value;

            return Insert(orderEntity);
        }

        return order;
    }

    public decimal GetTotalValueWithoutDiscount(LSCoreIdRequest request)
    {
        var order = Queryable()
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

        orderItemManager.Delete(new DeleteOrderItemRequest()
        {
            OrderId = currentOrder.Id,
            ProductId = request.ProductId
        });
    }

    public OrdersInfoDto GetOrdersInfo()
    {
        if (CurrentUser == null)
            throw new LSCoreNotFoundException();
        
        var orders = Queryable()
            .Include(x => x.User)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Where(x => x.CreatedBy == CurrentUser.Id && x.IsActive && x.Status == OrderStatus.Collected)
            .ToList();
        
        var user = Queryable<UserEntity>()
            .First(x => x.Id == CurrentUser.Id);

        return new OrdersInfoDto()
        {
            User = user.Username,
            NumberOfOrders = orders.Count,
            TotalDiscountValue = orders.Sum(order => order.Items.Sum(item => (item.PriceWithoutDiscount - item.Price) * item.Quantity * (item.VAT / 100 + 1)))
        };
    }
            
    public OrderGetSingleDto GetSingle(GetSingleOrderRequest request)
    {
        var order = Queryable()
            .Where(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Include(x => x.OrderOneTimeInformation)
            .Include(x => x.Referent)
            .Include(x => x.User)
            .FirstOrDefault();

        if (order == null)
            throw new LSCoreNotFoundException();

        if ((order.OrderOneTimeInformation == null && order.User.Id != CurrentUser?.Id) ||
            order.Status == OrderStatus.Open)
            throw new LSCoreBadRequestException();

        return order.ToDto<OrderEntity, OrderGetSingleDto>();
    }
}