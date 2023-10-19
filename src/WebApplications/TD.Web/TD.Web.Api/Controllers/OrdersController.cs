using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Core.Framework;
using TD.Web.Contracts.Dtos.Orders;
using TD.Web.Contracts.Enums;
using TD.Web.Contracts.Interfaces.IManagers;

namespace TD.Web.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        
        public OrdersController(IOrderManager orderManager, IHttpContextAccessor httpContextAccessor)
        {
            _orderManager = orderManager;
            _orderManager.SetContextInfo(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("/order")]
        [Authorization(UserType.User, UserType.Admin)]
        public Response<OrderGetDto> Get()
        {
            return _orderManager.GetCurrentUserOrder();
        }
    }
}
