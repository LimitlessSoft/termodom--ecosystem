using System.Net;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.Http
{
    public class Response : IResponse
    {
        public HttpStatusCode Status { get; set; }
        public bool NotOk => Status != HttpStatusCode.OK;
    }

    public class Response<T> : IResponse<T>
    {
        public T? Payload { get; set; }
        public HttpStatusCode Status { get; set; }
        public bool NotOk => Status != HttpStatusCode.OK;
    }
}
