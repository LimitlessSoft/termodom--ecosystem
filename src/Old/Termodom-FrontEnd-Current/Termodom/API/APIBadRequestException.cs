namespace Termodom.API
{
	/// <summary>
	/// API je vratio gresku 400 > Los parametar
	/// </summary>
	public class APIBadRequestException : System.Exception
	{
		public string Message { get; set; }

		public APIBadRequestException() { }

		public APIBadRequestException(string message)
		{
			this.Message = message;
		}
	}
}
