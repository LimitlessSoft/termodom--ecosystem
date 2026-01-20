namespace TD.Office.Common.Contracts.Dtos.ModuleHelp;

public class ModuleHelpDto
{
	public string UserText { get; set; }
	public long? UserHelpId { get; set; }
	public string SystemText { get; set; }
	public long? SystemHelpId { get; set; }
	public bool CanEditSystemText { get; set; }
}
