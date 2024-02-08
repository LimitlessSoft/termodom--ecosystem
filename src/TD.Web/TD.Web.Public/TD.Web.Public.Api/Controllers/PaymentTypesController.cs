using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Api.Controllers
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
        public LSCoreListResponse<PaymentTypeGetDto> GetMultiple() =>
            _paymentTypeManager.GetMultiple();
    }
}
