using Microsoft.AspNetCore.Mvc;
using LSCore.Contracts.Http;
using TDBrain_v3;

namespace TD.OfficeServer.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("/ping")]
        public LSCoreResponse<string> Ping() => new LSCoreResponse<string>("Pong!");

        [HttpGet]
        [Route("/test-sms")]
        public LSCoreResponse<string> TestSms()
        {
            GSMModem.QueueSMS(new List<string>() {"0693691472"}, "Hello test");
            return new LSCoreResponse<string>();
        }
    }
}