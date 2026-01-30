namespace TD.Web.Admin.Contracts.Requests.AiContent;

public class GenerateContentRequest
{
	public string? BaseContent { get; set; }
	public string? Style { get; set; }
	public int? MaxLength { get; set; }
	public bool FormatAsHtml { get; set; }
}
