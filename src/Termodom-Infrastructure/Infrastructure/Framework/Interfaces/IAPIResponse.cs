using System.Net;

namespace Infrastructure.Framework.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAPIResponse
    {
        /// <summary>
        /// Http status code of the response
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }
    }
}
