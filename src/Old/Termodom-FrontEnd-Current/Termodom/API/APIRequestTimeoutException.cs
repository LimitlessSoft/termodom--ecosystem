using System;

namespace Termodom.API
{
	/// <summary>
	/// Veza sa API-jem nije uspostavljena
	/// </summary>
	public class APIRequestTimeoutException : Exception
	{
		public APIRequestFailedLog Log { get; set; }

		public APIRequestTimeoutException() { }

		public APIRequestTimeoutException(APIRequestFailedLog log)
		{
			Log = log;
		}
	}
}
