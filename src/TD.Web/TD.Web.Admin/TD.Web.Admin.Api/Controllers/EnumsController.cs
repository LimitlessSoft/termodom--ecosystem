using LSCore.Contracts.Dtos;
using LSCore.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[LSCoreAuthorize]
[ApiController]
[Permissions(Permission.Access)]
public class EnumsController(IEnumManager enumManager) : ControllerBase
{
    [HttpGet]
    [Route("/order-statuses")]
    public List<LSCoreIdNamePairDto> GetOrderStatuses() => enumManager.GetOrderStatuses();

    [HttpGet]
    [Route("/user-types")]
    public List<LSCoreIdNamePairDto> GetUserTypes() => enumManager.GetUserTypes();

    [HttpGet]
    [Route("/product-group-types")]
    public List<LSCoreIdNamePairDto> GetProductGroupTypes() => enumManager.GetProductGroupTypes();

    [HttpGet]
    [Route("/product-stock-types")]
    public List<LSCoreIdNamePairDto> GetProductStockTypes() => enumManager.GetProductStockTypes();

    [HttpGet]
    [Route("/calculator-types")]
    public List<LSCoreIdNamePairDto> GetCalculatorTypes() => enumManager.GetCalculatorTypes();
}
