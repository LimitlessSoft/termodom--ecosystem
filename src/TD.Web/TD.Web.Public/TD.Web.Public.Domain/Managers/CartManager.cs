using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Dtos;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.Cart;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Cart;
using TD.Web.Public.Contrats.Interfaces.IManagers;

namespace TD.Web.Public.Domain.Managers
{
    public class CartManager : LSCoreBaseManager<CartManager>, ICartManager
    {
        private readonly IOrderManager _orderManager;
        private readonly IProductPriceManager _productPriceManager;
        private readonly IProductManager _productManager;
        private readonly IProductPriceGroupLevelManager _productPriceGroupLevelManager;

        public CartManager(ILogger<CartManager> logger, WebDbContext dbContext, IOrderManager orderManager, IProductPriceManager productPriceManager, IProductManager productManager, IProductPriceGroupLevelManager productPriceGroupLevelManager, IHttpContextAccessor httpContextAccessor) : base(logger, dbContext)
        {
            _orderManager = orderManager;
            _orderManager.SetContext(httpContextAccessor.HttpContext);
            _productPriceManager = productPriceManager;
            _productPriceManager.SetContext(httpContextAccessor.HttpContext);
            _productManager = productManager;
            _productManager.SetContext(httpContextAccessor.HttpContext);
            _productPriceGroupLevelManager = productPriceGroupLevelManager;
            _productPriceGroupLevelManager.SetContext(httpContextAccessor.HttpContext);
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

            #region Re-calculating prices for each item in order

            // Define total cart value buffer so we can calculate prices for each item based on one time price calculation
            var totalCartValue = decimal.Zero;

            // Calculate value of the cart if user is not logged
            // If user is logged, we do not need this calculation because we will calculate prices based on user level
            if(CurrentUser == null)
                foreach (var item in orderWithItems.Items)
                {
                    totalCartValue += _productPriceManager.GetProductPrice(new Contracts.Requests.ProductPrices.GetProductPriceRequest()
                    {
                        ProductId = item.ProductId
                    }).Payload!.MinPrice;
                }

            // foreach item in order, calculate price based on user level or one time price calculation
            foreach(var item in orderWithItems.Items)
            {
                var priceResponse = _productPriceManager.GetProductPrice(new Contracts.Requests.ProductPrices.GetProductPriceRequest()
                {
                    ProductId = item.ProductId
                });

                if (CurrentUser == null)
                    item.Price = PricesHelpers.CalculateOneTimeCartPrice(priceResponse.Payload!.MinPrice, priceResponse.Payload!.MaxPrice, totalCartValue);
                else
                {
                    var userPriceResponse = ExecuteCustomQuery<GetUsersProductPricesRequest, UserPricesDto>(new GetUsersProductPricesRequest()
                    {
                        UserId = CurrentUser.Id,
                        ProductId = item.ProductId
                    });
                    response.Merge(userPriceResponse);
                    if(response.NotOk)
                        return response;

                    item.Price = userPriceResponse.Payload!.PriceWithoutVAT;
                }
            }
            #endregion

            response.Payload = orderWithItems.ToDto<CartGetDto, OrderEntity>();
            return response;
        }
    }
}
