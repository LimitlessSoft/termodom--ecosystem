namespace TD.Web.Admin.Contracts.Requests.AiContent;

public class ValidateMetaRequest
{
	public string? MetaTitle { get; set; }
	public string? MetaDescription { get; set; }
	public Dictionary<string, string>? Context { get; set; }
}
