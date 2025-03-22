using LSCore.Repository.Contracts;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.Entities;

public class SettingEntity : LSCoreEntity
{
	public SettingKey Key { get; set; }
	public string Value { get; set; }
}
