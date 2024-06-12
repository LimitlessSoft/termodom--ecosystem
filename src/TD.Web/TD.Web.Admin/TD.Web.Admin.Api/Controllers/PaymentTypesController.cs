using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[ApiController]
public class PaymentTypesController (IPaymentTypeManager paymentTypeManager) : ControllerBase
{
    [HttpGet]
    [Route("/payment-types")]
    public List<PaymentTypesGetDto> GetMultiple() =>
        paymentTypeManager.GetMultiple();
}