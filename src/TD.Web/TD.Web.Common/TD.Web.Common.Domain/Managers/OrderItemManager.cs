using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
{
    public class OrderItemManager : LSCoreBaseManager<OrderItemManager, OrderItemEntity>, IOrderItemManager
    {
        private readonly WebDbContext _webDbContext;
        public OrderItemManager(ILogger<OrderItemManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
            _webDbContext = dbContext;
        }

        public void AddProductToCart(OrderItemEntity request) => Insert(request);

        public bool ItemExists(int productId, int userId, string oneTimeHash)
        {
            var order = _webDbContext.Orders.First(x => x.IsActive && (userId == 0 || x.UserId == userId) && (oneTimeHash == String.Empty || x.OneTimeHash == oneTimeHash));
            return _webDbContext.OrderItems.Any(x => x.ProductId == productId && x.OrderId == order.Id && x.IsActive);
        }

    }
}
