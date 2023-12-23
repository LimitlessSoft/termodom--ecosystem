using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Common.Contracts.Helpers.Orders;
using TD.Web.Common.Contracts.Requests.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using LSCore.Contracts.Requests;

namespace TD.Web.Common.Domain.Managers
{
    public class OrderManager : LSCoreBaseManager<OrderManager, OrderEntity>, IOrderManager
    {
        private readonly IOrderItemManager _orderItemManager;

        public OrderManager(ILogger<OrderManager> logger, WebDbContext dbContext, IOrderItemManager orderItemManager, IHttpContextAccessor httpContextAccessor)
        : base(logger, dbContext)
        {
            _orderItemManager = orderItemManager;
            _orderItemManager.SetContext(httpContextAccessor.HttpContext);
        }

        public LSCoreResponse AddItem(OrdersAddItemRequest request)
        {
            var response = new LSCoreResponse();

            var qProductResponse = Queryable<ProductEntity>();
            response.Merge(qProductResponse);
            if (response.NotOk)
                return response;

            var product = qProductResponse.Payload!
                .Where(x => x.Id == request.ProductId && x.IsActive)
                .Include(x => x.Price)
                .FirstOrDefault();
            if(product == null)
                return LSCoreResponse.NotFound();

            var orderResponse = GetOrCreateCurrentOrder(request.OneTimeHash);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;

            var orderItemExistsResponse = _orderItemManager.Exists(new Common.Contracts.Requests.OrderItems.OrderItemExistsRequest()
            {
                OrderId = orderResponse.Payload!.Id,
                ProductId = product.Id
            });
            response.Merge(orderItemExistsResponse);
            if (response.NotOk)
                return response;

            if(orderItemExistsResponse.Payload == true)
                return LSCoreResponse.BadRequest();

            var insertResponse = _orderItemManager.Insert(new OrderItemEntity()
            {
                VAT = product.VAT,
                OrderId = orderResponse.Payload.Id,
                ProductId = request.ProductId,
                Quantity = request.Quantity,
                Price = 0, // Price will be calculated later when user enter the cart
                PriceWithoutDiscount = product.Price.Max,
            });
            response.Merge(insertResponse);

            return response;
        }

        /// <inheritdoc/>
        public LSCoreResponse<OrderEntity> GetOrCreateCurrentOrder(string? oneTimeHash = null)
        {
            var response = new LSCoreResponse<OrderEntity>();

            var orderResponse = First(x =>
                x.IsActive &&
                x.Status == Contracts.Enums.OrderStatus.Open &&
                (CurrentUser == null ?
                    (string.IsNullOrWhiteSpace(oneTimeHash) ? false : x.OneTimeHash == oneTimeHash) :
                    x.CreatedBy == CurrentUser.Id));

            if(orderResponse.Status == System.Net.HttpStatusCode.NotFound)
            {
                var orderEntity = new OrderEntity();

                orderEntity.Status = Contracts.Enums.OrderStatus.Open;
                if(CurrentUser == null)
                    orderEntity.OneTimeHash = OrdersHelpers.GenerateOneTimeHash();
                else
                    orderEntity.CreatedBy = CurrentUser.Id;

                var insertResponse = Insert(orderEntity);
                response.Merge(insertResponse);
                if (response.NotOk)
                    return response;

                response.Payload = orderEntity;
                return response;
            }

            response.Merge(orderResponse);
            if(response.NotOk)
                return response;

            response.Payload = orderResponse.Payload;
            return response;
        }

        public LSCoreResponse<decimal> GetTotalValueWithoutDiscount(LSCoreIdRequest request)
        {
            var response = new LSCoreResponse<decimal>();

            var qOrderResponse = Queryable(x => x.Id == request.Id);
            response.Merge(qOrderResponse);
            if (response.NotOk)
                return response;

            var order = qOrderResponse.Payload!
                .Include(x => x.Items)
                .FirstOrDefault();

            if(order == null)
                return LSCoreResponse<decimal>.NotFound();

            var totalValue = 0m;

            order.Items.ForEach(x =>
            {
                totalValue += x.PriceWithoutDiscount * x.Quantity;
            });

            response.Payload = totalValue;
            return response;
        }

    }
}
