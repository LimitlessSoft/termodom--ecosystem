using System;
using System.Net.Http;

namespace Termodom.API
{
	public class APIRequestFailedLogItem
	{
		public DateTime Time { get; set; }
		public HttpRequestMessage Request { get; set; }
		public HttpResponseMessage Response { get; set; }
	}
}
