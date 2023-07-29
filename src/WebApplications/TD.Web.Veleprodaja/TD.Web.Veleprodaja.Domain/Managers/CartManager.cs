using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Exceptions;
using TD.Core.Contracts.Extensions;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.Web.Veleprodaja.Contracts.DtoMappings;
using TD.Web.Veleprodaja.Contracts.Dtos.Orders;
using TD.Web.Veleprodaja.Contracts.Entities;
using TD.Web.Veleprodaja.Contracts.Enums.Orders;
using TD.Web.Veleprodaja.Contracts.IManagers;
using TD.Web.Veleprodaja.Contracts.Requests.Orders;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Domain.Managers
{
    public class CartManager : BaseManager<CartManager>, ICartManager
    {
        public CartManager(ILogger<CartManager> logger, VeleprodajaDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response<OrdersGetDto> Get()
        {
            if (ContextUser == null)
                throw new NotAuthenticatedException();

            var user = First<User>(x => x.Username == ContextUser.GetName());
            var order = FirstOrDefault<Order>(x => x.UserId == user.Id && x.Status == OrderStatus.Created);
            if (order == null)
                order = Save<Order, OrdersPutRequest>(new OrdersPutRequest()
                {
                    UserId = user.Id,
                    Status = OrderStatus.Created
                });

            order = Queryable<Order>()
                .Include(x => x.Items)
                .First(x => x.Id == order.Id);

            return new Response<OrdersGetDto>(order.ToOrderGetDto());
        }
    }
}
