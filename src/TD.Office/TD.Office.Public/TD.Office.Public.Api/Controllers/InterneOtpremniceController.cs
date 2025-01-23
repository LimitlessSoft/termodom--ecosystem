using Microsoft.AspNetCore.Mvc;
using TD.Office.InterneOtpremnice.Client;

namespace TD.Office.Public.Api.Controllers;

public class InterneOtpremniceController(TDOfficeInterneOtpremniceClient microserviceApi)
    : ControllerBase
{
    [HttpGet]
    [Route("/interne-otpremnice")]
    public async Task<IActionResult> GetMultiple() => Ok(await microserviceApi.GetMultipleAsync());
}
