using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Http;
using LSCore.Framework;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class PaymentTypesController : ControllerBase
    {
        private readonly IPaymentTypeManager _paymentTypeManager;
        
        public PaymentTypesController(IPaymentTypeManager paymentTypeManager)
        {
            _paymentTypeManager = paymentTypeManager;
        }
        
        [HttpGet]
        [Route("/payment-types")]
        public LSCoreListResponse<PaymentTypesGetDto> GetMultiple() =>
            _paymentTypeManager.GetMultiple();
    }
}