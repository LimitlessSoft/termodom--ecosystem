using LSCore.Framework.Attributes;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Dtos.PaymentTypes;
using TD.Web.Common.Contracts.Attributes;
using Microsoft.AspNetCore.Authorization;
using TD.Web.Common.Contracts.Enums;
using Microsoft.AspNetCore.Mvc;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuthorize]
[ApiController]
[Permissions(Permission.Access)]
public class PaymentTypesController (IPaymentTypeManager paymentTypeManager) : ControllerBase
{
    [HttpGet]
    [Route("/payment-types")]
    public List<PaymentTypesGetDto> GetMultiple() =>
        paymentTypeManager.GetMultiple();
}