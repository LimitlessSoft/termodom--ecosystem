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
    public GetSpecifikacijaNovcaDto GetCurrent() =>
        specifikacijaNovcaManager.GetCurrent();

    [HttpGet]
    [Route("/specifikacija-novca/{Id}")]
    public GetSpecifikacijaNovcaDto GetSingle([FromRoute] GetSingleSpecifikacijaNovcaRequest request) =>
        specifikacijaNovcaManager.GetSingle(request);

    [HttpPut]
    [Route("/specifikacija-novca/{Id}")]
    public void Save([FromRoute] long Id, [FromBody] SaveSpecifikacijaNovcaRequest request)
    {
        request.Id = Id;
        specifikacijaNovcaManager.Save(request);
    }

    [HttpGet]
    [Route("/specifikacija-novca-next")]
    public GetSpecifikacijaNovcaDto GetNext([FromQuery] GetNextSpecifikacijaNovcaRequest request) =>
        specifikacijaNovcaManager.GetNext(request);

    [HttpGet]
    [Route("/specifikacija-novca-prev")]
    public GetSpecifikacijaNovcaDto GetPrevious([FromQuery] GetPrevSpecifikacijaNovcaRequest request) =>
        specifikacijaNovcaManager.GetPrev(request);
}
