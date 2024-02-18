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

        public LSCoreResponse<OrdersGetDto> GetSingle(OrdersGetSingleRequest request)
        {
            var response = new LSCoreResponse<OrdersGetDto>();

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
                return LSCoreResponse<OrdersGetDto>.NotFound();

            response.Payload = order.ToDto<OrdersGetDto, OrderEntity>();
            return response;
        }

        public LSCoreResponse PutStoreId(OrdersPutStoreIdRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;
            
            orderResponse.Payload!.StoreId = request.StoreId;
            response.Merge(Update(orderResponse.Payload));
            
            return response;
        }

        public LSCoreResponse PutStatus(OrdersPutStatusRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;
            
            orderResponse.Payload!.Status = request.Status;
            response.Merge(Update(orderResponse.Payload));
            
            return response;
        }

        public LSCoreResponse PutPaymentTypeId(OrdersPutPaymentTypeIdRequest request)
        {
            var response = new LSCoreResponse();
            
            var orderResponse = First(x => x.OneTimeHash == request.OneTimeHash && x.IsActive);
            response.Merge(orderResponse);
            if (response.NotOk)
                return response;
            
            orderResponse.Payload!.PaymentTypeId = request.PaymentTypeId;
            response.Merge(Update(orderResponse.Payload));
            
            return response;
        }
    }
}
