using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Dtos.PaymentTypes;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class PaymentTypesController(IPaymentTypeManager paymentTypeManager) : ControllerBase
{
	[HttpGet]
	[Route("/payment-types")]
	public List<PaymentTypeGetDto> GetMultiple() => paymentTypeManager.GetMultiple();
}
