using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class ModuleHelpEntity : LSCoreEntity
{
	public ModuleType ModuleType { get; set; }
	public string Text { get; set; }
}
