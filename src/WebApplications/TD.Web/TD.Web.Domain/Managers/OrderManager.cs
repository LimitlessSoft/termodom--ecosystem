using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.DtoMappings.Orders;
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
            var response = new Response<OrdersGetDto>();
            var entityResponse = First(x => x.Status == Contracts.Enums.OrderStatus.Open);
            response.Merge(entityResponse);
            if (response.NotOk)
            {
                var orderEntity = new OrderEntity();

                orderEntity.Status = Contracts.Enums.OrderStatus.Open;
                orderEntity.UserId = CurrentUser.Id;
                orderEntity.Date = DateTime.UtcNow;

                Insert(orderEntity);

                response.Payload = orderEntity.toDto();
                return response;
            }
            response.Payload = entityResponse.Payload.toDto();
            return response;
        }
    }
}
