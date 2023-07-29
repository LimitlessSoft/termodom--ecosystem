using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;
using TD.FE.TDOffice.Contracts.Dtos.Ping;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.Ping;

namespace TD.FE.TDOffice.Api.Controllers
{
    [ApiController]
    public class PingController : Controller
    {
        private readonly ILogger<PingController> _logger;
        private readonly IPingManager _pingManager;

        public PingController(ILogger<PingController> logger, IPingManager pingManager)
        {
            _logger = logger;
            _pingManager = pingManager;

            _logger.LogInformation("Logger is available by default because of dependency injection. Just catch it inside class constructor.");
            _logger.LogInformation("Use this to log something into console (output in VS or Logs in Docker)");
        }

        [HttpGet]
        [Route("/ping/raw")]
        public string Raw()
        {
            return @"This is some raw response (not wrapped into Response<TPayload> class).
                All responses need to be wrapped into Response<TPayload> but this is just to show it is possible for some critical scenario.";
        }

        [HttpGet]
        [Route("/ping")]
        public Response<GetPingDto> Get([FromQuery]PingGetRequest request)
        {
            return _pingManager.Get(request);
        }


        [HttpPut]
        [Route("/ping")]
        public Response Put([FromBody] PingPutRequest request)
        {
            return _pingManager.Put(request);
        }

    }
}
