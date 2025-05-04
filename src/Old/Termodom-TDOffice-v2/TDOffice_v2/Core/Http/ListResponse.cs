using System;
using System.Collections.Generic;
using System.Net;
using TDOffice_v2.Core.Http.Interfaces;

namespace TDOffice_v2.Core.Http
{
	public class ListResponse<TEntity> : IListResponse<TEntity>
	{
		public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
		public bool NotOk => Convert.ToInt16(Status).ToString()[0] != '2';
		public List<TEntity> Payload { get; set; } = new List<TEntity>();
		public List<string>? Errors { get; set; } = null;

		public ListResponse() { }

		public ListResponse(List<TEntity> payload)
		{
			Payload = payload;
		}

		public static ListResponse<TEntity> NotImplemented()
		{
			return new ListResponse<TEntity>() { Status = HttpStatusCode.NotImplemented };
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
			return new ListResponse<TEntity>() { Status = HttpStatusCode.InternalServerError };
		}

		public static ListResponse<TEntity> NoContent()
		{
			return new ListResponse<TEntity>() { Status = HttpStatusCode.NoContent };
		}
	}
}
