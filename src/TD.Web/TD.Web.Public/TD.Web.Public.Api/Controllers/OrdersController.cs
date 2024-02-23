using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Framework;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.Orders;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Orders;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        public OrdersController(IOrderManager orderManager,IHttpContextAccessor httpContextAccessor) 
        {
            _orderManager = orderManager;
            _orderManager.SetContext(httpContextAccessor.HttpContext);
        }
        [LSCoreAuthorization]
        [HttpPost]
        [Route("/orders")]
        public LSCoreSortedPagedResponse<OrdersGetDto> GetMultiple([FromBody]GetMultipleOrdersRequest request) =>
            _orderManager.GetMultiple(request);

        [HttpGet]
        [Route("/orders/{OneTimeHash}")]
        public LSCoreResponse<OrderGetSingleDto> GetSingle([FromRoute]GetSingleOrderRequest request) =>
            _orderManager.GetSingle(request);
    }
}
