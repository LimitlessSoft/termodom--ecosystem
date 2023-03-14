using System;
using System.Net.Http;

namespace TDOffice_v2.API
{
    public class APIRequestTimeoutExceptionArguments
    {
        public DateTime Time { get; set; }
        public HttpRequestMessage Request { get; set; }
        public HttpResponseMessage Response { get; set; }
    }
}
