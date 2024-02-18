using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Framework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Orders;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Orders;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    [LSCoreAuthorization]
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

        [HttpGet]
        [Route("/orders/{OneTimeHash}")]
        public LSCoreResponse<OrdersGetDto> GetSingle([FromRoute] OrdersGetSingleRequest request) =>
            _orderManager.GetSingle(request);
        
        [HttpPut]
        [Route("/orders/{OneTimeHash}/storeId/{StoreId}")]
        public LSCoreResponse PutStoreId([FromRoute] OrdersPutStoreIdRequest request) =>
            _orderManager.PutStoreId(request);
        
        [HttpPut]
        [Route("/orders/{OneTimeHash}/status/{status}")]
        public LSCoreResponse PutStoreId([FromRoute] OrdersPutStatusRequest request) =>
            _orderManager.PutStatus(request);
        
        [HttpPut]
        [Route("/orders/{OneTimeHash}/paymentTypeId/{PaymentTypeId}")]
        public LSCoreResponse PutStoreId([FromRoute] OrdersPutPaymentTypeIdRequest request) =>
            _orderManager.PutPaymentTypeId(request);
    }
}
