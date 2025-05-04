using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.API
{
	public class APIBadRequestException : Exception
	{
		public new string Message { get; set; }

		public APIBadRequestException() { }

		public APIBadRequestException(string message)
		{
			this.Message = message;
		}
	}
}
