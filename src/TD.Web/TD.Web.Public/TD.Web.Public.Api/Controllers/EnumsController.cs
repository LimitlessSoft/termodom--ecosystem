using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Attributes;
using Microsoft.AspNetCore.Authorization;
using TD.Web.Common.Contracts.Enums;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Dtos;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
[Permissions(Permission.Access)]
public class EnumsController (IEnumManager enumManager) : ControllerBase
{
    [HttpGet]
    [Route("/product-stock-types")]
    public List<LSCoreIdNamePairDto> GetProductStockTypes() =>
        enumManager.GetProductStockTypes();
}