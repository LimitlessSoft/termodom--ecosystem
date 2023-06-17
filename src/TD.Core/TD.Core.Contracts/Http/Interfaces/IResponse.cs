using System.Net;

namespace TD.Core.Contracts.Http.Interfaces
{
    public interface IResponse
    {
        HttpStatusCode Status { get; set; }
        bool NotOk { get; }
        List<string>? Errors { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T? Payload { get; set; }
    }
}
