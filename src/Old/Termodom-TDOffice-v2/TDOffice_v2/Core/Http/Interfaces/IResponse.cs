using System.Collections.Generic;
using System.Net;

namespace TDOffice_v2.Core.Http.Interfaces
{
    public interface IResponse
    {
        HttpStatusCode Status { get; set; }
        bool NotOk { get; }
        List<string> Errors { get; set; }
    }

    public interface IResponse<T> : IResponse
    {
        T? Payload { get; set; }
    }
}
