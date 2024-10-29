using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Api.Controllers;

[Authorize]
[ApiController]
[Permissions(Permission.Access, Permission.PartneriRead)]
public class ProracuniController(IProracunManager proracunManager) : ControllerBase
{
    [HttpGet]
    [Route("/proracuni")]
    public IActionResult GetMultiple([FromQuery] ProracuniGetMultipleRequest request) =>
        Ok(proracunManager.GetMultiple(request));

    [HttpPost]
    [Route("/proracuni")]
    public IActionResult Create([FromBody] ProracuniCreateRequest request)
    {
        proracunManager.Create(request);
        return Ok();
    }

    [HttpGet]
    [Route("/proracuni/{Id}")]
    public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
        Ok(proracunManager.GetSingle(request));

    [HttpPut]
    [Route("/proracuni/{Id}/state")]
    public IActionResult PutState(
        [FromRoute] LSCoreIdRequest idRequest,
        [FromBody] ProracuniPutStateRequest request
    )
    {
        request.Id = idRequest.Id;
        proracunManager.PutState(request);
        return Ok();
    }

    [HttpPut]
    [Route("/proracuni/{Id}/ppid")]
    public IActionResult PutPPID(
        [FromRoute] LSCoreIdRequest idRequest,
        [FromBody] ProracuniPutPPIDRequest request
    )
    {
        request.Id = idRequest.Id;
        proracunManager.PutPPID(request);
        return Ok();
    }
}
