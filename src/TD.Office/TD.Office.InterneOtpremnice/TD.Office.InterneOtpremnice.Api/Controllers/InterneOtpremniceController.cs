using Microsoft.AspNetCore.Mvc;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Api.Controllers;

public class InterneOtpremniceController(IInterneOtpremniceManager manager) : ControllerBase
{
    [HttpPost]
    [Route("interne-otpremnice")]
    public IActionResult CreateInterneOtpremnice(
        [FromBody] InterneOtpremniceCreateRequest request
    ) => Ok(manager.Create(request));

    [HttpGet]
    [Route("/interne-otpremnice")]
    public async Task<IActionResult> GetInterneOtpremnice([FromQuery] GetMultipleRequest request) =>
        Ok(await manager.GetMultipleAsync(request));
}
