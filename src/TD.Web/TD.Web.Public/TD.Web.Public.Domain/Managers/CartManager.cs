using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using Microsoft.AspNetCore.Http;
using LSCore.Contracts.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Requests.Cart;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using LSCore.Domain.Validators;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Helpers;

namespace TD.Web.Public.Domain.Managers
{
    public class CartManager : LSCoreBaseManager<CartManager>, ICartManager
    {
        private readonly IOrderManager _orderManager;

        public CartManager(ILogger<CartManager> logger, WebDbContext dbContext, IOrderManager orderManager, IHttpContextAccessor httpContextAccessor) : base(logger, dbContext)
        {
            _orderManager = orderManager;
            _orderManager.SetContext(httpContextAccessor.HttpContext);
        }

        public LSCoreResponse Checkout(CheckoutRequest request)
        {
            var response = new LSCoreResponse();

            if(request.IsRequestInvalid(response))
                return response;
            var currentOrderResponse = _orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);
            response.Merge(currentOrderResponse);
            if (response.NotOk)
                return response;

            var recalculateResponse = ExecuteCustomCommand(new RecalculateAndApplyOrderItemsPricesCommandRequest()
            {
                Id = currentOrderResponse.Payload!.Id,
                UserId = CurrentUser?.Id
            });

            if (currentOrderResponse.Payload!.Items.IsEmpty())
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            
            response.Merge(recalculateResponse);
            if (response.NotOk)
                return response;

            #region Entity Mapping
            if (CurrentUser == null)
                currentOrderResponse.Payload!.OrderOneTimeInformation = new OrderOneTimeInformationEntity()
                {
                    Name = request.Name,
                    Mobile = request.Mobile
                };
            currentOrderResponse.Payload!.Status = OrderStatus.PendingReview;
            currentOrderResponse.Payload.StoreId = request.StoreId;
            currentOrderResponse.Payload!.Note = request.Note;
            currentOrderResponse.Payload!.PaymentTypeId = request.PaymentTypeId;
            #endregion

            var orderResponse = Update(currentOrderResponse.Payload);
            response.Merge(orderResponse);

            return response;
        }

        public LSCoreResponse<CartGetDto> Get(CartGetRequest request)
        {
            var response = new LSCoreResponse<CartGetDto>();

            var orderResponse = _orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;

            var qOrderWithItemsResponse = Queryable<OrderEntity>();
            response.Merge(qOrderWithItemsResponse);
            if (response.NotOk)
                return response;

            var orderWithItems = qOrderWithItemsResponse.Payload!
                .Where(x => x.IsActive &&
                    x.Id == orderResponse.Payload!.Id)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Unit)
                .FirstOrDefault();
            if (orderWithItems == null)
                return LSCoreResponse<CartGetDto>.NotFound();

            // Recalculate and apply outdated prices if needed
            var recalculateResponse = ExecuteCustomCommand(new RecalculateAndApplyOrderItemsPricesCommandRequest()
            {
                Id = orderWithItems.Id,
                UserId = CurrentUser?.Id
            });
            response.Merge(recalculateResponse);
            if (response.NotOk)
                return response;

            response.Payload = orderWithItems.ToDto<CartGetDto, OrderEntity>();
            return response;
        }

        public LSCoreResponse<CartGetCurrentLevelInformationDto> GetCurrentLevelInformation(CartCurrentLevelInformationRequest request)
        {
            var response = new LSCoreResponse<CartGetCurrentLevelInformationDto>();

            var orderResponse = _orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);

            var qOrderWithItemsResponse = Queryable<OrderEntity>();
            response.Merge(qOrderWithItemsResponse);
            if (response.NotOk)
                return response;

            var orderWithItems = qOrderWithItemsResponse.Payload!
                .Where(x => x.IsActive &&
                    x.Id == orderResponse.Payload!.Id)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Unit)
                .FirstOrDefault();

            if(orderWithItems == null)
                return LSCoreResponse<CartGetCurrentLevelInformationDto>.NotFound();

            if (CurrentUser != null || orderWithItems.Items.IsEmpty())
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                return response;
            }
            var totalCartValueWithoutDiscount = orderWithItems.Items.Sum(x => x.Product.Price.Max * x.Quantity);

            response.Payload = new CartGetCurrentLevelInformationDto()
            {
                CurrentLevel = PricesHelpers.CalculateCartLevel(totalCartValueWithoutDiscount),
                NextLevelValue = PricesHelpers.CalculateValueToNextLevel(totalCartValueWithoutDiscount)
            };

            return response;
        }
    }
}
