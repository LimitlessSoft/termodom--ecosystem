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
    }
}
