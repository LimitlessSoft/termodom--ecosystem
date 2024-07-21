using TD.Web.Admin.Contracts.Requests.Units;
using TD.Web.Admin.Contracts.Dtos.Units;
using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[Authorize]
[ApiController]
[Permissions(Permission.Access)]
public class UnitsController (IUnitManager unitManager) : ControllerBase
{
    [HttpGet]
    [Route("/units/{id}")]
    public UnitsGetDto Get([FromRoute] int id) =>
        unitManager.Get(new LSCoreIdRequest() { Id = id });

    [HttpGet]
    [Route("/units")]
    public List<UnitsGetDto> GetMultiple() => 
        unitManager.GetMultiple();

    [HttpPut]
    [Route("/units")]
    public long Save([FromBody] UnitSaveRequest request) =>
        unitManager.Save(request);

    [HttpDelete]
    [Route("/units/{Id}")]
    public void Delete([FromRoute] LSCoreIdRequest request) =>
        unitManager.Delete(request);
}