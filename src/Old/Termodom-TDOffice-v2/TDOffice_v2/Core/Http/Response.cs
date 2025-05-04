using System;
using System.Collections.Generic;
using System.Net;
using TDOffice_v2.Core.Http.Interfaces;

namespace TDOffice_v2.Core.Http
{
	public class Response : IResponse
	{
		public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
		public bool NotOk => Convert.ToInt16(Status).ToString()[0] != '2';
		public List<string> Errors { get; set; } = null;

		public static Response NotImplemented()
		{
			return new Response() { Status = HttpStatusCode.NotImplemented };
		}

		public static Response BadRequest()
		{
			return BadRequest(null);
		}

		public static Response BadRequest(params string[]? errorMessages)
		{
			return new Response()
			{
				Status = HttpStatusCode.BadRequest,
				Errors = errorMessages == null ? null : new List<string>(errorMessages)
			};
		}

		public static Response InternalServerError()
		{
			return new Response() { Status = HttpStatusCode.InternalServerError };
		}

		public static Response NoContent()
		{
			return new Response() { Status = HttpStatusCode.NoContent };
		}
	}

	public class Response<TPayload> : IResponse<TPayload>
	{
		public Response() { }

		public Response(TPayload payload)
		{
			Payload = payload;
		}

		public TPayload? Payload { get; set; }
		public bool NotOk => Convert.ToInt16(Status).ToString()[0] != '2';
		public HttpStatusCode Status { get; set; } = HttpStatusCode.OK;
		public List<string>? Errors { get; set; } = null;

		public static Response<TPayload> NotImplemented()
		{
			return new Response<TPayload>() { Status = HttpStatusCode.NotImplemented };
		}

		public static Response<TPayload> BadRequest()
		{
			return BadRequest(null);
		}

		public static Response<TPayload> BadRequest(params string[]? errorMessages)
		{
			return new Response<TPayload>()
			{
				Status = HttpStatusCode.BadRequest,
				Errors = errorMessages == null ? null : new List<string>(errorMessages)
			};
		}

		public static Response<TPayload> InternalServerError()
		{
			return new Response<TPayload>() { Status = HttpStatusCode.InternalServerError };
		}

		public static Response<TPayload> NoContent()
		{
			return new Response<TPayload>() { Status = HttpStatusCode.NoContent };
		}
	}
}
