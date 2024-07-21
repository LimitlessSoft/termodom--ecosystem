using TD.Web.Admin.Contracts.Interfaces.IManagers;
using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Dtos;
using Microsoft.AspNetCore.Authorization;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[Authorize]
[ApiController]
[Permissions(Permission.Access)]
public class EnumsController (IEnumManager enumManager) : ControllerBase
{
    [HttpGet]
    [Route("/order-statuses")]
    public List<LSCoreIdNamePairDto> GetOrderStatuses() =>
        enumManager.GetOrderStatuses();
        
    [HttpGet]
    [Route("/user-types")]
    public List<LSCoreIdNamePairDto> GetUserTypes() =>
        enumManager.GetUserTypes();
        
    [HttpGet]
    [Route("/product-group-types")]
    public List<LSCoreIdNamePairDto> GetProductGroupTypes() =>
        enumManager.GetProductGroupTypes();
}