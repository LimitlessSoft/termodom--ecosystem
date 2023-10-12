using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.Web.Contracts.Dtos.Orders;
using TD.Web.Contracts.Interfaces.IManagers;

namespace TD.Web.Api.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        
        public OrdersController(IOrderManager orderManager, IHttpContextAccessor httpContextAccessor)
        {
            _orderManager = orderManager;
            _orderManager.SetContextInfo(httpContextAccessor.HttpContext);
        }

        [Authorize]
        [HttpGet]
        [Route("/order")]
        public Response<OrdersGetDto> Get()
        {
            return _orderManager.GetCurrentUserOrder();
        }
    }
}
