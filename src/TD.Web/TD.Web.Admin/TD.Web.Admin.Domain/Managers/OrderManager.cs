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
                var userInformationResponse = ExecuteCustomQuery<OrderGetUserInformationRequest, OrderUserInformationDto>(new OrderGetUserInformationRequest()
                {
                    UserId = order.CreatedBy
                });
                response.Merge(userInformationResponse);
                if (response.NotOk)
                    return response;

                response.Payload.UserInformation = userInformationResponse.Payload!;
            }
            
            if(order.Referent != null)
            {
                var referatInformationResponse = ExecuteCustomQuery<OrderGetUserInformationRequest, OrderUserInformationDto>(new OrderGetUserInformationRequest()
                {
                    UserId = order.Referent
                });
                response.Merge(referatInformationResponse);
                if (response.NotOk)
                    return response;

                response.Payload.ReferentName = referatInformationResponse.Payload!.Name;
            }
            #endregion
            return response;
        }
    }
}
