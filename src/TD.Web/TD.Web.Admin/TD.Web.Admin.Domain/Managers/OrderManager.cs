using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.DtoMappings.Orders;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class OrderManager : LSCoreBaseManager<OrderManager, OrderEntity>, IOrderManager
    {
        public OrderManager(ILogger<OrderManager> logger, WebDbContext dbContext)
        : base(logger, dbContext)
        {
        }

        public LSCoreResponse<OrderGetDto> GetCurrentUserOrder()
        {
            var response = new LSCoreResponse<OrderGetDto>();
            var entityResponse = First(x => x.Status == Common.Contracts.Enums.OrderStatus.Open && x.IsActive && x.CreatedBy == CurrentUser.Id);
            if (entityResponse.Status == System.Net.HttpStatusCode.NotFound)
            {
                var orderEntity = new OrderEntity();

                orderEntity.Status = Common.Contracts.Enums.OrderStatus.Open;
                orderEntity.UserId = CurrentUser.Id;
                orderEntity.Date = DateTime.UtcNow;

                Insert(orderEntity);

                response.Payload = orderEntity.toDto();
                return response;
            }

            response.Merge(entityResponse);
            if (response.NotOk)
                return response;

            response.Payload = entityResponse.Payload.toDto();
            return response;
        }
    }
}
