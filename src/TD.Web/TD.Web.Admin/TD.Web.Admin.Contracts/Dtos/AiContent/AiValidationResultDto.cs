namespace TD.Web.Admin.Contracts.Dtos.AiContent;

public class AiValidationResultDto
{
	public bool IsValid { get; set; }
	public string? OriginalValue { get; set; }
	public string? SuggestedValue { get; set; }
	public List<string> Issues { get; set; } = [];
	public List<string> Suggestions { get; set; } = [];
	public double ConfidenceScore { get; set; }
}
