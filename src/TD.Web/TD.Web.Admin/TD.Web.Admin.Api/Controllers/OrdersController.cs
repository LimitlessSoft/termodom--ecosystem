using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Orders;
using TD.Web.Admin.Contracts.Dtos.Orders;
using LSCore.Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Http;
using LSCore.Framework;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderManager _orderManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OrdersController(IOrderManager orderManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            
            _orderManager = orderManager;
            _orderManager.SetContext(_httpContextAccessor.HttpContext!);
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
        public LSCoreResponse PutPaymentTypeId([FromRoute] OrdersPutPaymentTypeIdRequest request) =>
            _orderManager.PutPaymentTypeId(request);
        
        [HttpPut]
        [Route("/orders/{OneTimeHash}/occupy-referent")]
        public LSCoreResponse PutOccupyReferent([FromRoute] OrdersPutOccupyReferentRequest request) =>
            _orderManager.PutOccupyReferent(request);

        [HttpPost]
        [Route("/orders/{OneTimeHash}/forward-to-komercijalno")]
        public Task<LSCoreResponse> PostForwardToKomercijalno([FromRoute] string oneTimeHash, [FromBody] OrdersPostForwardToKomercijalnoRequest request)
        {
            request.OneTimeHash = oneTimeHash;
            return _orderManager.PostForwardToKomercijalnoAsync(request);
        }

        [HttpPost]
        [Route("/orders/{OneTimeHash}/unlink-from-komercijalno")]
        public LSCoreResponse PostUnlinkFromKomercijalno([FromRoute] OrdersPostUnlinkFromKomercijalnoRequest request) =>
            _orderManager.PostUnlinkFromKomercijalno(request);
    }
}
