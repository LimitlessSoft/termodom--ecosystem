using Microsoft.AspNetCore.Mvc;
using TD.Core.Contracts.Http;

namespace Demo.Api.Controllers
{
    [ApiController]
    public class PingController : ControllerBase
    {
        [HttpGet]
        [Route("/ping")]
        public Response Get()
        {
            var response = new Response();
            return response;
        }

        [HttpGet]
        [Route("/ping1")]
        public Response<string> Get1()
        {
            return new Response<string>("Hello 1");
        }

        [HttpGet]
        [Route("/ping2")]
        public ListResponse<string> Get2()
        {
            var list = new List<string>();
            for (int i = 0; i < 10; i++)
                list.Add(i.ToString());
            return new ListResponse<string>(list);
        }


    }
}
