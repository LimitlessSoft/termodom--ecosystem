using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.API
{
	public class APIRequestTImeoutException : Exception
	{
		public List<APIRequestTimeoutExceptionArguments> Arguments { get; set; }

		public APIRequestTImeoutException() { }

		public APIRequestTImeoutException(List<APIRequestTimeoutExceptionArguments> args)
		{
			this.Arguments = args;
		}
	}
}
