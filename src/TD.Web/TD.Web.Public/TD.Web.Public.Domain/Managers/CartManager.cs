using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;

namespace TD.Web.Public.Domain.Managers
{
    public class CartManager : LSCoreBaseManager<CartManager>, ICartManager
    {
        private readonly IOrderManager _orderManager;

        public CartManager(ILogger<CartManager> logger, WebDbContext dbContext, IOrderManager orderManager) : base(logger, dbContext)
        {
            _orderManager = orderManager;
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

            response.Payload = orderWithItems.ToDto<CartGetDto, OrderEntity>();
            return response;
        }
    }
}
