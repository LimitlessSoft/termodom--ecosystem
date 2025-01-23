using Microsoft.AspNetCore.Mvc;
using TD.Office.InterneOtpremnice.Client;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.Public.Api.Controllers;

public class InterneOtpremniceController(TDOfficeInterneOtpremniceClient microserviceApi)
    : ControllerBase
{
    [HttpGet]
    [Route("/interne-otpremnice")]
    public async Task<IActionResult> GetMultiple([FromQuery] GetMultipleRequest request) =>
        Ok(await microserviceApi.GetMultipleAsync(request));
}
