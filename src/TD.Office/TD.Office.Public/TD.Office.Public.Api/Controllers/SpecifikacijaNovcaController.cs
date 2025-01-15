using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.SpecifikacijaNovca;

namespace TD.Office.Public.Api.Controllers;
[ApiController]
[Permissions(Permission.Access)]
public class SpecifikacijaNovcaController(ISpecifikacijaNovcaManager specifikacijaNovcaManager)
    : ControllerBase
{
    [HttpGet]
    [Route("/specifikacija-novca")]
    public async Task<GetSpecifikacijaNovcaDto> GetCurrent() =>
        await specifikacijaNovcaManager.GetCurrentAsync();

    [HttpGet]
    [Route("/specifikacija-novca/{Id}")]
    public async Task<GetSpecifikacijaNovcaDto> GetSingle([FromRoute] GetSingleSpecifikacijaNovcaRequest request) =>
        await specifikacijaNovcaManager.GetSingleAsync(request);

    [HttpPut]
    [Route("/specifikacija-novca/{Id}")]
    public void Save([FromRoute] long Id, [FromBody] SaveSpecifikacijaNovcaRequest request)
    {
        request.Id = Id;
        specifikacijaNovcaManager.Save(request);
    }

    [HttpGet]
    [Route("/specifikacija-novca-next")]
    public async Task<GetSpecifikacijaNovcaDto> GetNextAsync([FromQuery] GetNextSpecifikacijaNovcaRequest request) =>
        await specifikacijaNovcaManager.GetNextAsync(request);

    [HttpGet]
    [Route("/specifikacija-novca-prev")]
    public async Task<GetSpecifikacijaNovcaDto> GetPreviousAsync([FromQuery] GetPrevSpecifikacijaNovcaRequest request) =>
        await specifikacijaNovcaManager.GetPrevAsync(request);
}
