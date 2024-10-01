using Microsoft.AspNetCore.Mvc;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Calculator;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class CalculatorController(ICalculatorManager calculatorManager)
    : ControllerBase
{
    [HttpGet]
    [Route("/calculator-items")]
    public IActionResult GetCalculatorItems([FromQuery]GetCalculatorItemsRequest request) =>
        Ok(calculatorManager.GetCalculatorItems(request));
}