using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.Dtos.Ping;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.FE.TDOffice.Contracts.Requests.Ping;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class PingManager : BaseManager<PingManager>, IPingManager
    {
        public PingManager(ILogger<PingManager> logger) : base(logger)
        {
        }

        public Response<GetPingDto> Get(PingGetRequest request)
        {
            var response = new Response<GetPingDto>();

            var dto = new GetPingDto();

            if (string.IsNullOrWhiteSpace(request.SomeFilter1))
                dto.Value = "This is get request without any parameter!";
            else
                dto.Value = $"This is get request and it has some parameter passed! Parameter value: {request.SomeFilter1}";

            response.Payload = dto;
            return response;
        }

        public Response Put(PingPutRequest request)
        {
            // Here i do not put anything in payload. It may be useful for something to not return anything

            #region Validating request input (not correct way, just to show easier way to return bad request)
            var response = new Response();
            if(string.IsNullOrWhiteSpace(request.Value1))
            {
                response.Status = System.Net.HttpStatusCode.BadRequest;
                response.Errors = new List<string>()
                {
                    "You must pass value1 in request!"
                };
                return response;
            }

            if(string.IsNullOrWhiteSpace(request.Value2))
            {
                // This is same as above but more compact
                return Response.BadRequest("You must pass value2 in request");
            }
            #endregion

            // These two validations should be placed in validator. Example is below
            #region Validating request input (correct way)

            #endregion
            return response;
        }
    }
}
