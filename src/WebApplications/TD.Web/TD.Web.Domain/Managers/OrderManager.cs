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

        public Response<OrderGetDto> GetCurrentUserOrder()
        {
            var response = new Response<OrderGetDto>();
            var entityResponse = First(x => x.Status == Contracts.Enums.OrderStatus.Open && x.IsActive);
            response.Merge(entityResponse);
            if (response.Status == System.Net.HttpStatusCode.NotFound)
            {
                var orderEntity = new OrderEntity();

                orderEntity.Status = Contracts.Enums.OrderStatus.Open;
                orderEntity.UserId = CurrentUser.Id;
                orderEntity.Date = DateTime.UtcNow;

                Insert(orderEntity);

                response.Payload = orderEntity.toDto();
                return response;
            }
            if(response.NotOk)
                return response;

            response.Payload = entityResponse.Payload.toDto();
            return response;
        }
    }
}
