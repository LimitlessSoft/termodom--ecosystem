using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            _orderManager.SetContext(httpContextAccessor.HttpContext);
        }

        [HttpGet]
        [Route("/orders")]
        public LSCoreResponse<OrderGetDto> Get()
        {
            return _orderManager.GetCurrentUserOrder();
        }
    }
}
