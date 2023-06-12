using System.Net;

namespace TD.Core.Contracts.Http.Interfaces
{
    public interface IResponse
    {
        public HttpStatusCode Status { get; set; }
        public bool NotOk { get; }
    }

    public interface IResponse<T> : IResponse
    {
        public T? Payload { get; set; }
    }
}
