namespace TD.Web.Admin.Contracts.Dtos.Settings;

public class AiSettingsDto
{
	public string? ModelName { get; set; }
	public string? MaxTokens { get; set; }
	public string? Temperature { get; set; }

	// Validation prompts
	public string? ProductNameValidation { get; set; }
	public string? ProductDescriptionValidation { get; set; }
	public string? ProductShortDescriptionValidation { get; set; }
	public string? ProductMetaValidation { get; set; }

	// Generation prompts
	public string? ProductNameGenerate { get; set; }
	public string? ProductDescriptionGenerate { get; set; }
	public string? ProductShortDescriptionGenerate { get; set; }
	public string? ProductMetaGenerate { get; set; }

	// Blog prompts (future)
	public string? BlogTitleValidation { get; set; }
	public string? BlogContentValidation { get; set; }
	public string? BlogContentGenerate { get; set; }
}
