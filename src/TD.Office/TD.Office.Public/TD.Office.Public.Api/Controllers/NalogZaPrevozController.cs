using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using Microsoft.AspNetCore.Authorization;
using LSCore.Contracts.Requests;
using LSCore.Framework.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuthorize]
[ApiController]
public class NalogZaPrevozController (INalogZaPrevozManager nalogZaPrevozManager) : ControllerBase
{
    [HttpGet]
    [Route("/nalog-za-prevoz")]
    public IActionResult GetMultiple([FromQuery] GetMultipleNalogZaPrevozRequest request) =>
        Ok(nalogZaPrevozManager.GetMultiple(request));
        
    [HttpGet]
    [Route("/nalog-za-prevoz/{Id}")]
    public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
        Ok(nalogZaPrevozManager.GetSingle(request));

    [HttpPut]
    [Route("/nalog-za-prevoz")]
    public IActionResult SaveNalogZaPrevoz([FromBody] SaveNalogZaPrevozRequest request)
    {
        nalogZaPrevozManager.SaveNalogZaPrevoz(request);
        return Ok();
    }

    [HttpGet]
    [Route("/nalog-za-prevoz-referentni-dokument")]
    public async Task<IActionResult> GetReferentniDokument([FromQuery] GetReferentniDokumentNalogZaPrevozRequest request) =>
        Ok(await nalogZaPrevozManager.GetReferentniDokumentAsync(request));
}