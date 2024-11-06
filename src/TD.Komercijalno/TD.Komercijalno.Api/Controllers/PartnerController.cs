using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Partneri;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class PartnerController(IPartnerManager partnerManager) : ControllerBase
{
    [HttpGet]
    [Route("/partneri")]
    public IActionResult GetMultiple([FromQuery] PartneriGetMultipleRequest request) =>
        Ok(partnerManager.GetMultiple(request));

    [HttpPost]
    [Route("/partneri")]
    public IActionResult Create([FromBody] PartneriCreateRequest request) =>
        Ok(partnerManager.Create(request));

    [HttpGet]
    [Route("/partneri-duplikat")]
    public IActionResult GetDuplikat([FromQuery] PartneriGetDuplikatRequest request) =>
        Ok(partnerManager.GetDuplikat(request));

    [HttpGet]
    [Route("/partneri-kategorije")]
    public IActionResult GetKategorije() => Ok(partnerManager.GetKategorije());

    /// <summary>
    /// Vraca poslednji id manji od 100000 (mi smo ubagovali nesto i moramo ovako)
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [Route("/partneri-poslednji-id")]
    public IActionResult GetPoslednjiId() => Ok(partnerManager.GetPoslednjiId());

    [HttpGet]
    [Route("/partneri/{Id}")]
    public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
        Ok(partnerManager.GetSingle(request));
}
