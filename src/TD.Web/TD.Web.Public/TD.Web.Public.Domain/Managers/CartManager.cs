using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.OrderItems;
using TD.Web.Public.Contracts.Requests.Cart;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using Microsoft.AspNetCore.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Validators;
using TD.Web.Common.Repository;
using TD.Web.Common.Contracts;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;
using TD.OfficeServer.Contracts.Requests.SMS;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Domain.Managers
{
    public class CartManager : LSCoreBaseManager<CartManager>, ICartManager
    {
        private readonly IOrderManager _orderManager;
        private readonly IOfficeServerApiManager _officeServerApiManager;

        public CartManager(ILogger<CartManager> logger, WebDbContext dbContext, IOrderManager orderManager, IHttpContextAccessor httpContextAccessor, IOfficeServerApiManager officeServerApiManager)
            : base(logger, dbContext)
        {
            _orderManager = orderManager;
            _orderManager.SetContext(httpContextAccessor.HttpContext);
            
            _officeServerApiManager = officeServerApiManager;
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
            response.Merge(recalculateResponse);
            if (response.NotOk)
                return response;

            if (currentOrderResponse.Payload!.Items.IsEmpty())
                return LSCoreResponse.BadRequest();

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
            currentOrderResponse.Payload!.CheckedOutAt = DateTime.UtcNow;
            #endregion

            var orderResponse = Update(currentOrderResponse.Payload);
            response.Merge(orderResponse);

            #region QueueSMS
            var storeName = "Unknown";

            if (request.StoreId == -5)
            {
                storeName = "Dostava";
            }
            else
            {
                var store = Queryable<StoreEntity>()
                    .LSCoreFilters(x => x.Id == request.StoreId);
                storeName = store.Payload?.FirstOrDefault()?.Name ?? storeName;                    
            }
            
            _officeServerApiManager.SMSQueueAsync(new SMSQueueRequest()
            {
                Text = $"Nova pourzbina je zakljucena. Mesto preuzimanja: {storeName}",
            });
            #endregion

            return response;
        }

        public LSCoreResponse<CartGetDto> Get(CartGetRequest request)
        {
            var response = new LSCoreResponse<CartGetDto>();

            var orderResponse = _orderManager.GetOrCreateCurrentOrder(request.OneTimeHash);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;

            var qOrderWithItemsResponse = Queryable<OrderEntity>()
                .LSCoreFilters(x => x.IsActive && x.Id == orderResponse.Payload!.Id);
            response.Merge(qOrderWithItemsResponse);
            if (response.NotOk)
                return response;

            var orderWithItems = qOrderWithItemsResponse.Payload!
                .Include(x => x.User)
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
            response.Payload.FavoriteStoreId = orderWithItems.User.Id == 0 ? Constants.DefaultFavoriteStoreId : orderWithItems.User.FavoriteStoreId;
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
                .ThenInclude(x => x.Price)
                .FirstOrDefault();

            if(orderWithItems == null)
                return LSCoreResponse<CartGetCurrentLevelInformationDto>.NotFound();

            if (CurrentUser != null || orderWithItems.Items.IsEmpty())
                return LSCoreResponse<CartGetCurrentLevelInformationDto>.BadRequest();

            var totalCartValueWithoutDiscount = orderWithItems.Items.Sum(x => x.Price * x.Quantity);
            response.Payload = new CartGetCurrentLevelInformationDto()
            {
                CurrentLevel = PricesHelpers.CalculateCartLevel(totalCartValueWithoutDiscount),
                NextLevelValue = totalCartValueWithoutDiscount + PricesHelpers.CalculateValueToNextLevel(totalCartValueWithoutDiscount)
            };

            return response;
        }
    }
}
