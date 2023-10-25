using System.Net;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.Http
{
    public class Response : IResponse
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public bool NotOk => Convert.ToInt16(Status).ToString()[0] != '2';
        public List<string>? Errors { get; set; } = null;

        public Response()
        {

        }

        public Response(IResponse originResponse)
        {
            this.Status = originResponse.Status;
            this.Errors = originResponse.Errors;
        }

        public void Merge(IResponse response) => Status = response.NotOk ? response.Status == HttpStatusCode.NotFound ? HttpStatusCode.NotFound : HttpStatusCode.BadRequest : Status;
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
        public static Response NoContent()
        {
            return new Response()
            {
                Status = HttpStatusCode.NoContent
            };
        }
        public static Response NotFound()
        {
            return new Response()
            {
                Status = HttpStatusCode.NotFound
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
        public bool NotOk => Convert.ToInt16(Status).ToString()[0] != '2';
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public List<string>? Errors { get; set; } = null;

        public void Merge(IResponse response)
        {
            Status = response.NotOk ? response.Status == HttpStatusCode.NotFound ? HttpStatusCode.NotFound : HttpStatusCode.BadRequest : Status;
            
            if(Errors == null)
                Errors = new List<string>();

            if(response.Errors != null)
                Errors.AddRange(response.Errors);
        }
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
        public static Response<TPayload> InternalServerError(params string[] message)
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.InternalServerError,
                Errors = message.ToList()
            };
        }
        public static Response<TPayload> NoContent()
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.NoContent
            };
        }
        public static Response<TPayload> Forbidden()
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.Forbidden
            };
        }
        public static Response<TPayload> Unauthorized()
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.Unauthorized
            };
        }
        public static Response<TPayload> NotFound()
        {
            return new Response<TPayload>()
            {
                Status = HttpStatusCode.NotFound
            };
        }
    }
}
