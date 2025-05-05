namespace TD.OfficeServer.Contracts.Requests.SMS
{
	public class SMSQueueRequest
	{
		public List<string> Numbers { get; set; } = new List<string>();
		public string Text { get; set; }
	}
}
