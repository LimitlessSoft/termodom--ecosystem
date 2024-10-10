using LSCore.Contracts.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Calculator;

namespace TD.Web.Admin.Api.Controllers;

[Authorize]
[ApiController]
public class CalculatorItemsController(ICalculatorManager calculatorManager) : ControllerBase
{
    [HttpGet]
    [Route("/calculator-items")]
    public IActionResult GetCalculatorItems([FromQuery] GetCalculatorItemsRequest request) =>
        Ok(calculatorManager.GetCalculatorItems(request));

    [HttpPut]
    [Route("/calculator-items/{Id}/quantity")]
    public IActionResult UpdateCalculatorItemQuantity(
        [FromRoute] LSCoreIdRequest idRequest,
        [FromBody] UpdateCalculatorItemQuantityRequest request
    )
    {
        request.Id = idRequest.Id;
        calculatorManager.UpdateCalculatorItemQuantity(request);
        return Ok();
    }

    [HttpPost]
    [Route("/calculator-items/{Id}/mark-primary")]
    public IActionResult MarkAsPrimaryCalculatorItem([FromRoute] LSCoreIdRequest idRequest)
    {
        calculatorManager.MarkAsPrimaryCalculatorItem(idRequest);
        return Ok();
    }

    [HttpPut]
    [Route("/calculator-items-move")]
    public IActionResult MoveCalculatorItem([FromBody] MoveCalculatorItemRequest request)
    {
        calculatorManager.MoveCalculatorItem(request);
        return Ok();
    }

    [HttpDelete]
    [Route("/calculator-items/{Id}")]
    public IActionResult DeleteCalculatorItem([FromRoute] LSCoreIdRequest idRequest)
    {
        calculatorManager.DeleteCalculatorItem(idRequest);
        return Ok();
    }

    [HttpPost]
    [Route("/calculator-items")]
    public IActionResult AddCalculatorItem([FromBody] AddCalculatorItemRequest request)
    {
        calculatorManager.AddCalculatorItem(request);
        return Ok();
    }

    [HttpPut]
    [Route("/calculator-items/{Id}/classification")]
    public IActionResult UpdateCalculatorItemClassification(
        [FromRoute] LSCoreIdRequest idRequest,
        [FromBody] UpdateCalculatorItemClassificationRequest request
    )
    {
        request.Id = idRequest.Id;
        calculatorManager.UpdateCalculatorItemClassification(request);
        return Ok();
    }
}
