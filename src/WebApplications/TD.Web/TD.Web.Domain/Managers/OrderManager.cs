using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.Dtos.Orders;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
{
    public class OrderManager : BaseManager<OrderManager, OrderEntity>, IOrderManager
    {
        public OrderManager(ILogger<OrderManager> logger, WebDbContext dbContext)
           : base(logger, dbContext)
        {
        }

        public Response<OrdersGetDto> GetCurrentUserOrder()
        {
            throw new NotImplementedException();
        }
    }
}
