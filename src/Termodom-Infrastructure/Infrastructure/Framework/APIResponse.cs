using System;
using System.Linq;
using System.Net.Http;
using System.Net;
using Infrastructure.Exceptions;
using Newtonsoft.Json;
using Infrastructure.Framework.Interfaces;

namespace Infrastructure.Framework
{
    /// <summary>
    /// Class used to handle HttpResponseMessage without need to parse response content
    /// </summary>
    public class APIResponse : IAPIResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Main constructor
        /// </summary>
        /// <param name="statusCode"></param>
        public APIResponse(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        /// <summary>
        /// Initialize APIResponse object based on HttpResponseMessage object
        /// </summary>
        /// <param name="response"></param>
        public APIResponse(HttpResponseMessage response)
        {
            this.StatusCode = response.StatusCode;
        }
    }

    /// <summary>
    /// Class used to handle HttpResponseMessage with expected content
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public class APIResponse<TResponse> : APIResponse
    {
        /// <summary>
        /// Parsed content of the response
        /// </summary>
        public TResponse Body { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="statusCode"></param>
        public APIResponse(TResponse body, HttpStatusCode statusCode = HttpStatusCode.OK) : base(statusCode)
        {
            Body = body;
        }
        /// <summary>
        /// Initialize APIResponse object based on HttpResponseMessage object
        /// </summary>
        /// <param name="response"></param>
        public APIResponse(HttpResponseMessage response) : base(response)
        {
            if (!new[] { HttpStatusCode.OK, HttpStatusCode.Created }.Contains(StatusCode))
                throw new APIUnhandledException(response);

            if (response.Content.Headers.ContentLength == 0)
                throw new NullReferenceException("Response has no content!");

            var result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            var deserialized = JsonConvert.DeserializeObject<TResponse>(result);

            if (deserialized == null)
                throw new NullReferenceException("Response content has been deserialized but returned null!");

            Body = deserialized;
        }
    }
}
