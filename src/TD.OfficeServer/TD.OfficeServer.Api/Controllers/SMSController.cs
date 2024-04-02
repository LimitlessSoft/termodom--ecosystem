using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.OfficeServer.Contracts.IManagers;
using TD.OfficeServer.Contracts.Requests.SMS;

namespace TD.OfficeServer.Api.Controllers
{
    [ApiController]
    public class SMSController : ControllerBase
    {
        private readonly IConfigurationRoot _config;
        private readonly ISMSManager _smsManager;

        public SMSController(IConfigurationRoot configurationRoot, ISMSManager smsManager)
        {
            _config = configurationRoot;
            _smsManager = smsManager;
            _smsManager.ConnectionString = _config["ConnectionString_AG"]!;
        }

        [HttpPost]
        [Route("/SMS/Queue")]
        public LSCoreResponse Queue([FromBody] SMSQueueRequest request) =>
            _smsManager.Queue(request);
    }
}
