using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using static TD.Web.Admin.Contracts.Enums.SortColumnCodes.OrdersSortColumnCodes;

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

            return response;
        }
    }
}
