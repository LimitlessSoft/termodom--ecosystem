using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access)]
public class PaymentTypesController(IPaymentTypeManager paymentTypeManager) : ControllerBase
{
	[HttpGet]
	[Route("/payment-types")]
	public List<PaymentTypesGetDto> GetMultiple() => paymentTypeManager.GetMultiple();
}
