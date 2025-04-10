namespace TD.Office.MassSMS.Contracts.Requests;

public class MassQueueSmsRequest
{
	public string Message { get; set; }
	public List<string> PhoneNumbers { get; set; }
}
