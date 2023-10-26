using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers
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
        [Route("/orders")]
        public Response<OrderGetDto> Get()
        {
            return _orderManager.GetCurrentUserOrder();
        }
    }
}
