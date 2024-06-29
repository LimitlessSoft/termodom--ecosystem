using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class PaymentTypesController (IPaymentTypeManager paymentTypeManager) : ControllerBase
{
    [HttpGet]
    [Route("/payment-types")]
    public List<PaymentTypeGetDto> GetMultiple() =>
        paymentTypeManager.GetMultiple();
}