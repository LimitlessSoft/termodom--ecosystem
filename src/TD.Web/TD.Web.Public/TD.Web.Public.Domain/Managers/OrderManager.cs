using LSCore.Contracts.Extensions;
using LSCore.Contracts.Responses;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums.SortColumnCodes;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Orders;

namespace TD.Web.Public.Domain.Managers
{
    public class OrderManager : LSCoreBaseManager<OrderManager, OrderEntity>, IOrderManager
    {
        public OrderManager(ILogger<OrderManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(GetMultipleOrdersRequest request)
        {
            var response = new LSCoreSortedPagedResponse<OrdersGetDto>();

            var qResponse = Queryable();

            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            var orders = qResponse.Payload!
                .Include(x => x.User)
                .Include(x => x.Items)
                .ThenInclude(x => x.Product)
                .Where(x => x.IsActive && x.User.Id == CurrentUser.Id)
                .ToSortedAndPagedResponse(request, OrdersSortColumnCodes.OrdersSortRules);

            response.Merge(orders);
            if (response.NotOk)
                return response;
            
            return new LSCoreSortedPagedResponse<OrdersGetDto>(orders.Payload.ToDtoList<OrdersGetDto, OrderEntity>(),
                request,
                orders.Pagination.TotalElementsCount);
        }
    }
}
