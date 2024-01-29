using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Orders;

namespace TD.Web.Admin.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;

        public OrdersController(IOrderManager orderManager)
        {
            _orderManager = orderManager;
        }

        [HttpGet]
        [Route("/orders")]
        public LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple([FromQuery] OrdersGetMultipleRequest request) =>
            _orderManager.GetMultiple(request);
    }
}
