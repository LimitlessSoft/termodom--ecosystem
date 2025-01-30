using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Api.Controllers;

[ApiController]
[Permissions(Permission.Access, Permission.SpecifikacijaNovcaRead)]
public class SpecifikacijaNovcaController(ISpecifikacijaNovcaManager specifikacijaNovcaManager)
    : ControllerBase
{
    [HttpGet]
    [Route("/specifikacija-novca")]
    public async Task<IActionResult> GetCurrent() =>
        Ok(await specifikacijaNovcaManager.GetCurrentAsync());

    [HttpGet]
    [Route("/specifikacija-novca/{Id}")]
    [Permissions(Permission.SpecifikacijaNovcaPretragaPoBroju)]
    public async Task<IActionResult> GetSingle(
        [FromRoute] GetSingleSpecifikacijaNovcaRequest request
    ) => Ok(await specifikacijaNovcaManager.GetSingleAsync(request));

    [HttpPut]
    [Route("/specifikacija-novca/{Id}")]
    public IActionResult Save([FromRoute] long Id, [FromBody] SaveSpecifikacijaNovcaRequest request)
    {
        request.Id = Id;
        specifikacijaNovcaManager.Save(request);
        return Ok();
    }

    [HttpGet]
    [Route("/specifikacija-novca-next")]
    public async Task<IActionResult> GetNextAsync(
        [FromQuery] GetNextSpecifikacijaNovcaRequest request
    ) => Ok(await specifikacijaNovcaManager.GetNextAsync(request));

    [HttpGet]
    [Route("/specifikacija-novca-prev")]
    public async Task<IActionResult> GetPreviousAsync(
        [FromQuery] GetPrevSpecifikacijaNovcaRequest request
    ) => Ok(await specifikacijaNovcaManager.GetPrevAsync(request));

    [HttpGet]
    [Route("/specififikacija-novca-date")]
    public async Task<IActionResult> GetSpecifikacijaByDate(
        [FromQuery] GetSpecifikacijaByDateRequest request
    ) => Ok(await specifikacijaNovcaManager.GetSpecifikacijaByDate(request));
}
