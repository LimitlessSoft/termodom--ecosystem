namespace TD.Office.MassSMS.Contracts.Requests;

public class QueueSmsRequest
{
	public string PhoneNumber { get; set; }
	public string Message { get; set; }
}
