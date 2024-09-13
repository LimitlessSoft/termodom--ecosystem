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
    [Route("/partneri-kategorije")]
    public IActionResult GetKategorije() => Ok(partnerManager.GetKategorije());
}
