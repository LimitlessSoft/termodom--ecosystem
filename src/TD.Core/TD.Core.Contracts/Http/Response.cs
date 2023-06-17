using System.Net;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.Http
{
    public class Response : IResponse
    {
        public HttpStatusCode Status { get; set; }
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
            return new Response()
            {
                Status = HttpStatusCode.BadRequest
            };
        }
    }

    public class Response<T> : IResponse<T>
    {
        public T? Payload { get; set; }
        public HttpStatusCode Status { get; set; }
        public bool NotOk => Status != HttpStatusCode.OK;
        public List<string>? Errors { get; set; } = null;

        public static Response<T> NotImplemented()
        {
            return new Response<T>()
            {
                Status = HttpStatusCode.NotImplemented
            };
        }
        public static Response<T> BadRequest()
        {
            return new Response<T>()
            {
                Status = HttpStatusCode.BadRequest
            };
        }
    }
}
