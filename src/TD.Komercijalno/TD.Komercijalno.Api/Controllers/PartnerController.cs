using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Partneri;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class PartnerController(IPartnerManager partnerManager) : ControllerBase
{
    [HttpPost]
    [Route("/partneri")]
    public IActionResult Create(PartneriCreateRequest request) =>
        Ok(partnerManager.Create(request));
}
