using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Orders;
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

        public LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(OrdersGetMultipleRequest request)
        {
            var response = new LSCoreSortedPagedResponse<OrdersGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var orders = qResponse.Payload!
                .Where(x => x.IsActive &&
                    (request.Status == null || request.Status.Contains(x.Status)))
                .Include(x => x.User)
                .Include(x => x.OrderOneTimeInformation)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product);

            response.Payload = orders.ToDtoList<OrdersGetDto, OrderEntity>();
            return response;
        }

        public LSCoreResponse<OrderGetSingleDto> GetSingle(OrderGetSingleRequest request)
        {
            var response = new LSCoreResponse<OrderGetSingleDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var order = qResponse.Payload!
                .Where(x => x.OneTimeHash == request.OneTimeHash && x.IsActive)
                .Include(x => x.Items)
                    .ThenInclude(x => x.Product)
                .Include(x => x.OrderOneTimeInformation)
                .Include(x => x.Referent)
                .Include(x => x.User)
                .FirstOrDefault();

            if (order == null)
                return LSCoreResponse<OrderGetSingleDto>.NotFound();

            response.Payload = order.ToDto<OrderGetSingleDto, OrderEntity>();

            #region User information
            if(order.OrderOneTimeInformation != null)
            {
                response.Payload.UserInformation = new OrderUserInformationDto()
                {
                    Name = order.OrderOneTimeInformation.Name,
                    Mobile = order.OrderOneTimeInformation.Mobile
                };
            }
            else
            {
                response.Payload.UserInformation = new OrderUserInformationDto()
                {
                    Id = order.User?.Id,
                    Name = order.User?.Nickname,
                    Mobile = order.User?.Mobile
                };
            }
            
            #endregion
            return response;
        }
    }
}
