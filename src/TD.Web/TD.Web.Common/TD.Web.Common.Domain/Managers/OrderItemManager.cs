using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
{
    public class OrderItemManager : LSCoreBaseManager<OrderItemManager, OrderItemEntity>, IOrderItemManager
    {
        public OrderItemManager(ILogger<OrderItemManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<OrderItemEntity> Insert(OrderItemEntity request) => base.Insert(request);

        public LSCoreResponse<bool> Exists(OrderItemExistsRequest request)
        {
            var response = new LSCoreResponse<bool>();

            var qOrderItemResponse = Queryable();
            response.Merge(qOrderItemResponse);
            if (response.NotOk)
                return response;

            var orderItem = qOrderItemResponse.Payload!
                .Where(x => x.ProductId == request.ProductId && x.IsActive)
                .Include(x => x.Order)
                .FirstOrDefault();

            response.Payload = orderItem != null;
            return response;
        }

        public LSCoreResponse Delete(DeleteOrderItemRequest request)
        {
            var response = new LSCoreResponse();
            if (request.IsRequestInvalid(response))
                return response;

            var orderItemResponse = GetOrderItem(new GetOrderItemRequest()
            {
                OrderId = request.OrderId,
                ProductId = request.ProductId
            });
            response.Merge(orderItemResponse);
            if (response.NotOk)
                return response;

            return HardDelete(
                orderItemResponse.Payload!
            );
        }

        public LSCoreResponse<OrderItemEntity> GetOrderItem(GetOrderItemRequest request) =>
            First(x => x.OrderId == request.OrderId && x.ProductId == request.ProductId && x.IsActive);

        public LSCoreResponse ChangeQuantity(ChangeOrderItemQuantityRequest request)
        {
            var response = new LSCoreResponse();
            if (request.IsRequestInvalid(response))
                return response;

            var orderItemResponse = GetOrderItem(new GetOrderItemRequest()
            {
                OrderId = request.OrderId,
                ProductId = request.ProductId
            });
            response.Merge(orderItemResponse);
            if (response.NotOk)
                return response;

            orderItemResponse.Payload!.Quantity = request.Quantity;
            response.Merge(Update(orderItemResponse.Payload!));

            return response;
        }
    }
}
