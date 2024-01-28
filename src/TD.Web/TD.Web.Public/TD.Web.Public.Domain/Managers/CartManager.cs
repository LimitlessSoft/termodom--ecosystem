using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Common.Contracts.Interfaces.IManagers;
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

            var totalCartPrice = decimal.Zero;
            if(CurrentUser == null)
                foreach (var item in orderWithItems.Items)
                {
                    totalCartPrice += _productPriceManager.GetProductPrice(new Contracts.Requests.ProductPrices.GetProductPriceRequest()
                    {
                        ProductId = item.ProductId
                    }).Payload!.MinPrice;
                }

            foreach(var item in orderWithItems.Items)
            {
                var priceResponse = _productPriceManager.GetProductPrice(new Contracts.Requests.ProductPrices.GetProductPriceRequest()
                {
                    ProductId = item.ProductId
                });

                if (CurrentUser == null)
                    item.Price = PricesHelpers.CalculateOneTimeCartPrice(priceResponse.Payload!.MinPrice, priceResponse.Payload!.MaxPrice, totalCartPrice);
                else
                {
                    var productPriceGroup = _productManager.GetProductPriceGroup(new Contracts.Requests.Products.GetProductPriceGroupRequest()
                    {
                        ProductId = item.ProductId
                    });
                    var userLevel = _productPriceGroupLevelManager.GetUserLevel(new Contracts.Requests.ProductPriceGroupLevels.GetUserLevelRequest()
                    {
                        UserId = CurrentUser.Id,
                        ProductPriceGroupId = productPriceGroup.Payload
                    });
                    item.Price = PricesHelpers.CalculateProductPriceByLevel(priceResponse.Payload!.MinPrice, priceResponse.Payload!.MaxPrice,userLevel.Payload);
                }
            }

            response.Payload = orderWithItems.ToDto<CartGetDto, OrderEntity>();
            return response;
        }
    }
}
