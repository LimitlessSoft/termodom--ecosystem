using System.Net;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.Http
{
    public class ListResponse<TEntity> : IListResponse<TEntity>
    {
        public HttpStatusCode Status { get; set; }
        public bool NotOk => Status != HttpStatusCode.OK;
        public List<TEntity> Payload { get; set; } = new List<TEntity>();
        public List<string>? Errors { get; set; } = null;

        public static ListResponse<TEntity> NotImplemented()
        {
            return new ListResponse<TEntity>()
            {
                Status = HttpStatusCode.NotImplemented
            };
        }
        public static ListResponse<TEntity> BadRequest()
        {
            return new ListResponse<TEntity>()
            {
                Status = HttpStatusCode.BadRequest
            };
        }
    }
}
