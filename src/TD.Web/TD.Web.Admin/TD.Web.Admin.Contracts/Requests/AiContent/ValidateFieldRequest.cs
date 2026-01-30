namespace TD.Web.Admin.Contracts.Requests.AiContent;

public class ValidateFieldRequest
{
	public string Value { get; set; } = string.Empty;
	public Dictionary<string, string>? Context { get; set; }
}
