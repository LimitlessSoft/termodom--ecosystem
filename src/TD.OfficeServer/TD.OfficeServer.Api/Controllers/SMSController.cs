using TD.OfficeServer.Contracts.Requests.SMS;
using TD.OfficeServer.Contracts.IManagers;
using Microsoft.AspNetCore.Mvc;

namespace TD.OfficeServer.Api.Controllers;

[ApiController]
public class SmsController : ControllerBase
{
    private readonly ISmsManager _smsManager;

    public SmsController(IConfigurationRoot configurationRoot, ISmsManager smsManager)
    {
        _smsManager = smsManager;
        _smsManager.ConnectionString = configurationRoot["ConnectionString_AG"]!;
    }

    [HttpPost]
    [Route("/SMS/Queue")]
    public IActionResult Queue([FromBody] SMSQueueRequest request)
    {
        _smsManager.Queue(request);
        return Ok();
    }
}