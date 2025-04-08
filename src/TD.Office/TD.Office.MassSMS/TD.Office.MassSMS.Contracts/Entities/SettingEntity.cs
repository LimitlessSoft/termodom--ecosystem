using LSCore.Repository.Contracts;
using TD.Office.MassSMS.Contracts.Enums;

namespace TD.Office.MassSMS.Contracts.Entities;

public class SettingEntity : LSCoreEntity
{
	public Setting Setting { get; set; }
	public string Value { get; set; }
}
