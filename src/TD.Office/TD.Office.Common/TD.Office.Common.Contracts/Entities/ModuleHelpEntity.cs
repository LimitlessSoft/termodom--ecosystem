using LSCore.Repository.Contracts;
using TD.Office.Common.Contracts.Enums;

namespace TD.Office.Common.Contracts.Entities;

public class ModuleHelpEntity : LSCoreEntity
{
	public ModuleType ModuleType { get; set; }
	public string Text { get; set; }
}
