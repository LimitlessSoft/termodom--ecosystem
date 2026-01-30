namespace TD.Web.Admin.Contracts.Dtos.AiContent;

public class AiGeneratedContentDto
{
	public bool Success { get; set; }
	public string? GeneratedContent { get; set; }
	public string? HtmlContent { get; set; }
	public string? AlternativeContent { get; set; }
	public string? ErrorMessage { get; set; }
}
