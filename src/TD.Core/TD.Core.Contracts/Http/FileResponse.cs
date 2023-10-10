using Microsoft.AspNetCore.Http;
using System.Net;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.Http
{
    public class FileResponse : IResponse
    {
        public int ContentLength { get => Data == null ? 0 : Data.Length; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public Dictionary<string, string> Tags { get; set; }
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;

        public bool NotOk => Convert.ToInt16(Status).ToString()[0] != '2';
        public List<string>? Errors { get; set; } = null;

        public void Merge(IResponse response) => Status = response.NotOk ? response.Status == HttpStatusCode.NotFound ? HttpStatusCode.NotFound : HttpStatusCode.BadRequest : Status;
        public static FileResponse NotImplemented()
        {
            return new FileResponse()
            {
                Status = HttpStatusCode.NotImplemented
            };
        }
        public static FileResponse BadRequest()
        {
            return BadRequest(null);
        }
        public static FileResponse BadRequest(params string[]? errorMessages)
        {
            return new FileResponse()
            {
                Status = HttpStatusCode.BadRequest,
                Errors = errorMessages == null ? null : new List<string>(errorMessages)
            };
        }
        public static FileResponse InternalServerError()
        {
            return new FileResponse()
            {
                Status = HttpStatusCode.InternalServerError
            };
        }
        public static FileResponse NoContent()
        {
            return new FileResponse()
            {
                Status = HttpStatusCode.NoContent
            };
        }
        public static FileResponse NotFound()
        {
            return new FileResponse()
            {
                Status = HttpStatusCode.NotFound
            };
        }
    }
}
