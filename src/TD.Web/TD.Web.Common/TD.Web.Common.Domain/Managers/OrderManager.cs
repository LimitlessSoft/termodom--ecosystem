using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.DtoMappings.Orders;
using TD.Web.Common.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Common.Domain.Managers
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
            var entityResponse = First(x => x.Status == Contracts.Enums.OrderStatus.Open && x.IsActive && x.CreatedBy == CurrentUser.Id);
            if (entityResponse.Status == System.Net.HttpStatusCode.NotFound)
            {
                var orderEntity = new OrderEntity();

                orderEntity.Status = Contracts.Enums.OrderStatus.Open;
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

        public LSCoreResponse<OrderGetDto> GetOneTimeOrder(string OneTimeHash)
        {
            var response = new LSCoreResponse<OrderGetDto>();
            var entityResponse = First(x => x.Status == Contracts.Enums.OrderStatus.Open && x.IsActive && x.OneTimeHash == OneTimeHash && x.CreatedAt > DateTime.UtcNow.AddHours(-6));
            if (entityResponse.Status == System.Net.HttpStatusCode.NotFound)
            {
                var orderEntity = new OrderEntity();

                orderEntity.Status = Contracts.Enums.OrderStatus.Open;
                orderEntity.OneTimeHash = OneTimeHash;
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
