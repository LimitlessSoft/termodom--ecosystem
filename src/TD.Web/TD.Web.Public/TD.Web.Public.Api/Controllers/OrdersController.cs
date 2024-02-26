using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Orders;

namespace TD.Web.Public.Api.Controllers
{
    [LSCoreAuthorization]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        public OrdersController(IOrderManager orderManager,IHttpContextAccessor httpContextAccessor) 
        {
            _orderManager = orderManager;
            _orderManager.SetContext(httpContextAccessor.HttpContext);
        }

        [HttpPost]
        [Route("/orders")]
        public LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple(GetMultipleOrdersRequest request) =>
            _orderManager.GetMultiple(request);

        [LSCoreAuthorization]
        [HttpGet]
        [Route("/orders-info")]
        public LSCoreResponse<OrdersInfoDto> GetOrdersInfo() =>
            _orderManager.GetOrdersInfo();
    }
}
