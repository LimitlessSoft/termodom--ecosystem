using System;
using System.Net.Http;

namespace Infrastructure.Exceptions
{
    /// <summary>
    /// This exception is thrown when responses status code has not been handled by code
    /// </summary>
    public class APIUnhandledException : Exception
    {
        /// <summary>
        /// Full response message from which status code has not been handled
        /// </summary>
        public HttpResponseMessage Response { get; private set; }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="response"></param>
        public APIUnhandledException(HttpResponseMessage response) : base($"Unhandled API response '{response.StatusCode}'!")
        {
            this.Response = response;
        }
    }
}
