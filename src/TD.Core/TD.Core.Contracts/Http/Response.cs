using System.Net;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.Http
{
    public class Response : IResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public bool NotOk => Status != HttpStatusCode.OK;
        public List<string>? Errors { get; set; } = null;

        public static Response NotImplemented()
        {
            return new Response()
            {
                Status = HttpStatusCode.NotImplemented
            };
        }
        public static Response BadRequest()
        {
            return BadRequest(null);
        }
        public static Response BadRequest(params string[]? errorMessages)
        {
            return new Response()
            {
                Status = HttpStatusCode.BadRequest,
                Errors = errorMessages == null ? null : new List<string>(errorMessages)
            };
        }
        public static Response InternalServerError()
        {
            return new Response()
            {
                Status = HttpStatusCode.InternalServerError
            };
        }
    }

    public class Response<TPayload> : IResponse<TPayload>
    {
        public Response()
        {

        }

        public Response(TPayload payload)
        {
            Payload = payload;
        }
        public TPayload? Payload { get; set; }
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public bool NotOk => Status != HttpStatusCode.OK;
        public List<string>? Errors { get; set; } = null;

        public static Response<TPayload> NotImplemented()
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.NotImplemented
            };
        }
        public static Response<TPayload> BadRequest()
        {
            return BadRequest(null);
        }
        public static Response<TPayload> BadRequest(params string[]? errorMessages)
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.BadRequest,
                Errors = errorMessages == null ? null : new List<string>(errorMessages)
            };
        }
        public static Response<TPayload> InternalServerError()
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.InternalServerError
            };
        }
    }
}
