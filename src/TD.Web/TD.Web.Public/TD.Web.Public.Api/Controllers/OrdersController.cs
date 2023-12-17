using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Orders;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Common.Api.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        
        public OrdersController(IOrderManager orderManager, IHttpContextAccessor httpContextAccessor)
        {
            _orderManager = orderManager;
            _orderManager.SetContext(httpContextAccessor.HttpContext!);
        }

        [HttpGet]
        [Route("/orders")]
        public LSCoreResponse<OrderGetDto> Get() =>
            _orderManager.GetCurrentUserOrder();
    }
}
