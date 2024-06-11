using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Web.Common.Domain.Managers;

public class OrderItemManager (ILogger<OrderItemManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<OrderItemManager, OrderItemEntity>(logger, dbContext), IOrderItemManager
{
    public OrderItemEntity Insert(OrderItemEntity request) => base.Insert(request);

    public bool Exists(OrderItemExistsRequest request) =>
        Queryable()
            .Where(x => x.ProductId == request.ProductId && x.IsActive && x.OrderId == request.OrderId)
            .Include(x => x.Order)
            .Any();

    public void Delete(DeleteOrderItemRequest request)
    {
        request.Validate();
            
        HardDelete(GetOrderItem(new GetOrderItemRequest()
        {
            OrderId = request.OrderId,
            ProductId = request.ProductId
        }));
    }

    public OrderItemEntity GetOrderItem(GetOrderItemRequest request) =>
        Queryable().FirstOrDefault(x =>
            x.OrderId == request.OrderId && x.ProductId == request.ProductId && x.IsActive)
        ?? throw new LSCoreNotFoundException();

    public void ChangeQuantity(ChangeOrderItemQuantityRequest request)
    {
        request.Validate();

        var item = GetOrderItem(new GetOrderItemRequest()
        {
            OrderId = request.OrderId,
            ProductId = request.ProductId
        });
        item.Quantity = request.Quantity;
        Update(item);
    }
}