using System.Net;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.Http
{
    public class ListResponse<TEntity> : IListResponse<TEntity>
    {
        public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
        public bool NotOk => Status != HttpStatusCode.OK;
        public List<TEntity> Payload { get; set; } = new List<TEntity>();
        public List<string>? Errors { get; set; } = null;

        public ListResponse()
        {

        }

        public ListResponse(List<TEntity> payload)
        {
            Payload = payload;
        }

        public static ListResponse<TEntity> NotImplemented()
        {
            return new ListResponse<TEntity>()
            {
                Status = HttpStatusCode.NotImplemented
            };
        }
        public static ListResponse<TEntity> BadRequest()
        {
            return BadRequest(null);
        }
        public static ListResponse<TEntity> BadRequest(params string[]? errorMessages)
        {
            return new ListResponse<TEntity>()
            {
                Status = HttpStatusCode.BadRequest,
                Errors = errorMessages == null ? null : new List<string>(errorMessages)
            };
        }
        public static ListResponse<TEntity> InternalServerError()
        {
            return new ListResponse<TEntity>()
            {
                Status = HttpStatusCode.InternalServerError
            };
        }
    }
}
